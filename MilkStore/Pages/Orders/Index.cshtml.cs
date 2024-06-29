using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

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


        public IndexModel(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _orderDetailService = serviceProvider.GetRequiredService<IOrderDetailService>();
        }

        public void OnGet()
        {
            TotalPrice = 0;
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            foreach (var item in Cart) {
                TotalPrice += (item.Price * item.Quantity);
            }
            var accountNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            AccountName = accountNameClaim.Value;

        }

        public void OnPost()
        {
            //if (ModelState.IsValid)
            //{

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
                }


                HttpContext.Session.Remove("Cart");


             RedirectToPage("/Home/Product");
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid order attempt.");
            //    return Page();
            //}

        }




    }
}
