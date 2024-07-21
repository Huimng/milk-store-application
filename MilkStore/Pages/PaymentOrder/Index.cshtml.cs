using System;
using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            Cart.ForEach(item => {
                TotalPrice += (item.Price * item.Quantity);
            });
            Order = HttpContext.Session.GetObjectFromJson<Order>("Order");
            OrderContact = HttpContext.Session.GetObjectFromJson<OrderContact>("OrderContact");
            Order.Address = String.Format("{0} {1} {2} {3}", OrderContact.HouseNumber, OrderContact.District, OrderContact.Province, OrderContact.City);
            Order.GrandTotal = TotalPrice;
            if(Order == null || OrderContact == null)
            {
                return new JsonResult(String.Empty);
            }

            // create the request body
           
            JsonObject amount = new()
            {
                { "currency_code", "USD" },
                { "value", Order.GrandTotal }
            };

            JsonObject purchaseUnit1 = new JsonObject
            {
                { "amount", amount }
            };

            JsonArray purchaseUnits = new JsonArray
            {
                purchaseUnit1
            };
            JsonObject createOrderRequest = new()
            {
                { "intent", "CAPTURE" },
                {"purchase_units", purchaseUnits}
            };

            // get access token
            string orderId = String.Empty;
            using (var client = new HttpClient())
            {
                string accessToken = GetPaypalAccessToken();
                // send request
                string url = String.Format("{0}{1}", PaypalUrl, "/v2/checkout/orders");
                
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));

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
                        orderId = jsonResponse["id"]?.ToString() ?? String.Empty;

                        //save order in database

                        foreach (var item in Cart)
                        {
                            var checkProduct = _productService.GetProduct(item.ProductId);
                            if(item.Quantity > checkProduct.Quantity)
                            {
                                return new JsonResult(String.Empty);
                            }
                            //_orderDetailService.CreateOrderDetail(orderDetail);
                            //_productService.UpdateQuantityProduct(orderDetail.ProductId, orderDetail.Quantity);
                        }

                        Order.Status = OrderStatus.Pending;
                        Order.TotalDiscount = 0;
                        Order.SubTotal = 0;
                        Order.GrandTotal = TotalPrice;
                        Order.CartId = 0;
                        Order.Address = String.Format("{0} {1} {2} {3}", OrderContact.HouseNumber, OrderContact.District, OrderContact.Province, OrderContact.City);
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
                            OrderDetail orderDetail = new() {
                                OrderId = order.OrderId,
                                Quantity = item.Quantity,
                                TotalPrice = item.Price * item.Quantity,
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow,
                                ProductId = item.ProductId,
                            };
                            _orderDetailService.CreateOrderDetail(orderDetail);
                            _productService.UpdateQuantityProduct(orderDetail.ProductId, orderDetail.Quantity, Order.CreatedDate);
                        }
                    }
                }
            }
                
            return new JsonResult( new {Id = orderId});
        }

        public JsonResult OnPostCompleteOrder([FromBody] JsonObject data)
        {
                if(data ==null || data["orderID"]==null) return new JsonResult(String.Empty);
                var orderID = data["orderID"]!.ToString();

            // get access token
            string accessToken = GetPaypalAccessToken();
            string url = String.Format("{0}/v2/checkout/orders/{1}/capture", PaypalUrl, orderID);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(String.Empty, null, "application/json");

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

                return new JsonResult(String.Empty);
        }

        public JsonResult OnPostCancelOrder([FromBody] JsonObject data)
        {
            if (data == null || data["orderID"] == null) return new JsonResult(String.Empty);
            
            return new JsonResult(String.Empty);
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
                        accessToken = jsonResponse["access_token"]?.ToString() ?? String.Empty;
                    }
                }
            }
            return accessToken;
        }
    }
}
