using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MilkStore.Pages.Orders
{
    [Authorize(Roles = "Member")]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<CartItem> Cart { get; set; }
        [BindProperty]
        public OrderContact OrderContact { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public double TotalPrice { get; set; }

        public string AccountName { get; set; }

        private readonly IOrderContactService _orderContactService;

        private readonly IOrderService _orderService;

        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;

        public IndexModel(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _orderDetailService = serviceProvider.GetRequiredService<IOrderDetailService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }

        public void OnGet()
        {

            GetCartInOrder();
        }

        public void GetCartInOrder()
        {
            TotalPrice = 0;
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            foreach (var item in Cart)
            {
                TotalPrice += (item.Price * item.Quantity);
            }
            var accountNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            AccountName = accountNameClaim.Value;
        }

        public void OnPost()
        {
            //if (ModelState.IsValid)
            //{
           if( string.IsNullOrEmpty(OrderContact.CustomerName))
            {
                ModelState.AddModelError(string.Empty, "Invalid customer name attempt.");
                GetCartInOrder();
                Page();
                return;
            }
           if(!Regex.IsMatch(OrderContact.CustomerName, @"^[\p{L}\s]+$"))
            {
                ModelState.AddModelError(string.Empty, "Name Customer is not number");
                GetCartInOrder();
                Page();
                return;
            }
            if (string.IsNullOrEmpty(OrderContact.Phone))
            {
                ModelState.AddModelError(string.Empty, "Invalid phone attempt.");
                GetCartInOrder();
                Page();
                return;
            }
            if (!Regex.IsMatch(OrderContact.Phone, @"^\d+$"))
            {
                ModelState.AddModelError(string.Empty, "phone is number");
                GetCartInOrder();
                Page();
                return;
            }
            if (!Regex.IsMatch(OrderContact.Phone, @"^\d{10,11}$"))
            {
                ModelState.AddModelError(string.Empty, "Phone number must be 10 or 11 digits long.");
                GetCartInOrder();
                Page();
                return;
            }

            if (string.IsNullOrEmpty(OrderContact.City))
            {
                ModelState.AddModelError(string.Empty, "Invalid city attempt.");
                GetCartInOrder();
                Page();
                return;
            }
            if (string.IsNullOrEmpty(OrderContact.Province))
            {
                ModelState.AddModelError(string.Empty, "Invalid province attempt.");
                GetCartInOrder();
                Page();
                return;
            }
            if (string.IsNullOrEmpty(OrderContact.District))
            {
                ModelState.AddModelError(string.Empty, "Invalid district attempt.");
                GetCartInOrder();
                Page();
                return;
            }
            if (string.IsNullOrEmpty(OrderContact.HouseNumber))
            {
                ModelState.AddModelError(string.Empty, "Invalid house number attempt.");
                GetCartInOrder();
                Page();
                return;
            }

            TotalPrice = 0;
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            foreach (var item in Cart)
            {
                TotalPrice += (item.Price * item.Quantity);
            }


            if (Order.PaymentMethod == PaymentType.PayPal)
            {
                HttpContext.Session.SetObjectAsJson("Order", Order);
                HttpContext.Session.SetObjectAsJson("OrderContact", OrderContact);
                Response.Redirect("/PaymentOrder");
                return;
            }
            foreach (var item in Cart)
            {
                var checkproduct = _productService.GetProduct(item.ProductId);
                if (item.Quantity > checkproduct.Quantity)
                {
                    ModelState.AddModelError(string.Empty, "quantity very big");
                    GetCartInOrder();
                    Page();
                    return;
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
                    _productService.UpdateQuantityProduct(orderDetail.ProductId, orderDetail.Quantity);
            }


                HttpContext.Session.Remove("Cart");


            Response.Redirect("/Home/Product");
            return;
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid order attempt.");
            //    return Page();
            //}

        }




    }
}
