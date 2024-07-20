using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;

namespace MilkStore.Pages.PaymentOrder
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        public string PaypalClientId { get; set; } = "";
        private string PaypalSecret { get; set; } = "";
        public string PaypalUrl { get; set; } = "";

        public List<CartItem> Cart { get; set; }
        public Order Order { get; set; } 
        public OrderContact OrderContact { get; set; }

        private readonly IOrderContactService _orderContactService;

        private readonly IOrderService _orderService;

        private readonly IOrderDetailService _orderDetailService;

        private readonly IProductService _productService;

        public IndexModel(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            PaypalClientId = configuration["PaypalSettings:ClientId"]!;
            PaypalSecret = configuration["PaypalSettings:Secret"]!;
            PaypalUrl = configuration["PaypalSettings:Url"]!;
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _orderDetailService = serviceProvider.GetRequiredService<IOrderDetailService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        public void OnGet()
        {
            double TotalPrice = 0;
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            foreach (var item in Cart)
            {
                TotalPrice += (item.Price * item.Quantity);
            }
            Order = HttpContext.Session.GetObjectFromJson<Order>("Order");           
            OrderContact = HttpContext.Session.GetObjectFromJson<OrderContact>("OrderContact");
            Order.Address =OrderContact.HouseNumber + " " + OrderContact.District + " " + OrderContact.Province + " " + OrderContact.City;
            Order.GrandTotal = TotalPrice;
        }

        public JsonResult OnPostCreateOrder()
        {
            double TotalPrice = 0;
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            foreach (var item in Cart)
            {
                TotalPrice += (item.Price * item.Quantity);
            }
            Order = HttpContext.Session.GetObjectFromJson<Order>("Order");
            OrderContact = HttpContext.Session.GetObjectFromJson<OrderContact>("OrderContact");
            Order.Address = OrderContact.HouseNumber + " " + OrderContact.District + " " + OrderContact.Province + " " + OrderContact.City;
            Order.GrandTotal = TotalPrice;
            if(Order == null || OrderContact == null)
            {
                return new JsonResult("");
            }

            // create the request body
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", Order.GrandTotal);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            // get access token
            string accessToken = GetPaypalAccessToken();
            // send request
            string url = PaypalUrl + "/v2/checkout/orders";

            string orderId = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if(jsonResponse != null)
                    {
                        orderId = jsonResponse["id"]?.ToString() ?? "";

                        //save order in database

                        foreach (var item in Cart)
                        {
                            var checkproduct = _productService.GetProduct(item.ProductId);
                            if(item.Quantity > checkproduct.Quantity)
                            {
                                return new JsonResult("");
                            }
                            //_orderDetailService.CreateOrderDetail(orderDetail);
                            //_productService.UpdateQuantityProduct(orderDetail.ProductId, orderDetail.Quantity);
                        }

                        Order.Status = OrderStatus.Pending;
                        Order.TotalDiscount = 0;
                        Order.SubTotal = 0;
                        Order.GrandTotal = TotalPrice;
                        Order.CartId = 0;
                        Order.Address = OrderContact.HouseNumber + " " + OrderContact.District + " " + OrderContact.Province + " " + OrderContact.City;
                        Order.CreatedDate = DateTime.UtcNow;
                        Order.UpdatedDate = DateTime.UtcNow;
                        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
                        if (accountIdClaim != null && short.TryParse(accountIdClaim.Value, out short accountId))
                        {
                            Order.AccountId = accountId;
                        }
                        var order = _orderService.CreateOrder(Order);

                        OrderContact.OrderId = order.OrderId;
                        OrderContact.CreatedDate = DateTime.UtcNow;
                        OrderContact.UpdatedDate = DateTime.UtcNow;
                        _orderContactService.CreateOrderContact(OrderContact);

                        foreach (var item in Cart)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderId = order.OrderId;
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.TotalPrice = item.Price * item.Quantity;
                            orderDetail.CreatedDate = DateTime.UtcNow;
                            orderDetail.UpdatedDate = DateTime.UtcNow;
                            orderDetail.ProductId = item.ProductId;
                            _orderDetailService.CreateOrderDetail(orderDetail);
                            _productService.UpdateQuantityProduct(orderDetail.ProductId, orderDetail.Quantity, Order.CreatedDate);
                        }




                    }
                }
            }



                var response = new
                {
                Id = orderId
                };
            return new JsonResult(response);
        }

        public JsonResult OnPostCompleteOrder([FromBody] JsonObject data)
        {
                if(data ==null || data["orderID"]==null) return new JsonResult("");
                var orderID = data["orderID"]!.ToString();

            // get access token
            string accessToken = GetPaypalAccessToken();
            string url = PaypalUrl + "/v2/checkout/orders/" + orderID + "/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();
                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if(jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if(paypalOrderStatus == "COMPLETED")
                        {
                            //clear session
                            HttpContext.Session.Remove("Cart");
                            HttpContext.Session.Remove("Order");
                            HttpContext.Session.Remove("OrderContact");
                            return new JsonResult("success");
                        }
                    }
                }
            }

                return new JsonResult("");
        }

        public JsonResult OnPostCancelOrder([FromBody] JsonObject data)
        {
            if (data == null || data["orderID"] == null) return new JsonResult("");
            var orderID = data["orderID"]!.ToString();
            return new JsonResult("");
        }

        private string GetPaypalAccessToken()
        {
            string accessToken = "";
            string url = PaypalUrl + "/v1/oauth2/token";
            using (var client  = new HttpClient())
            {
                string credentials64 =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null
                    , "application/x-www-form-urlencoded");
                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }
            return accessToken;
        }
    }
}
