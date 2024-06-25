using BusinessLogics.Services;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkStore.Pages.Orders
{
    [Authorize(Roles = "Member")]
    public class GetOrdersModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderContactService _orderContactService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;


        public GetOrdersModel(IServiceProvider serviceProvider) 
        {
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
            _orderDetailService = serviceProvider.GetRequiredService<IOrderDetailService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        [BindProperty]
        public IList<Order> ListOrder { get; set; }


        public async Task OnGet()
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim != null && short.TryParse(accountIdClaim.Value, out short accountId))
            {

                ListOrder = _orderService.GetAllOrderByAccount(accountId);

                foreach (var order in ListOrder)
                {
                    foreach (var item in order.OrderDetails)
                    {
                        item.Product = _productService.GetProduct(item.ProductId);
                    }
                }
                

            }

        }
    }
}
