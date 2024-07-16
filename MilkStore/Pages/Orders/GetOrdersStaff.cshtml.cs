using BusinessLogics.Services;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkStore.Pages.Orders
{
    [Authorize(Roles = "Staff")]
    public class GetOrdersStaffModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public GetOrdersStaffModel(IServiceProvider serviceProvider)
        {
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        [BindProperty]
        public IList<Order> ListOrder { get; set; }
        [BindProperty]
        public Order CurrentOrder { get; set; }
        public async Task OnGet()
        {

            ListOrder = _orderService.GetAllOrder();

            foreach (var order in ListOrder)
            {
                foreach (var item in order.OrderDetails)
                {
                    item.Product = _productService.GetProduct(item.ProductId);
                }
            }
        }

        public IActionResult OnPostUpdateOrder()
        {
            if (string.IsNullOrEmpty(CurrentOrder.OrderContact.CustomerName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Name attempt.");
                return RedirectToPage("/Orders/GetOrdersStaff");
            }
            if (string.IsNullOrEmpty(CurrentOrder.OrderContact.Phone))
            {
                ModelState.AddModelError(string.Empty, "Invalid Phone attempt.");
                return RedirectToPage("/Orders/GetOrdersStaff");
            }
            if (string.IsNullOrEmpty(CurrentOrder.Address))
            {
                ModelState.AddModelError(string.Empty, "Invalid Address attempt.");
                return RedirectToPage("/Orders/GetOrdersStaff");
            }

            _orderService.UpdateOrder(CurrentOrder);
            return RedirectToPage("/Orders/GetOrdersStaff");
        }
    }
}
