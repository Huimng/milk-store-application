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
        private readonly IProductFeedbackService _productFeedbackService;


        public GetOrdersModel(IServiceProvider serviceProvider) 
        {
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
            _orderDetailService = serviceProvider.GetRequiredService<IOrderDetailService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _productFeedbackService = serviceProvider.GetRequiredService<IProductFeedbackService>();
        }
        [BindProperty]
        public IList<Order> ListOrder { get; set; }
        [BindProperty]
        public ProductFeedback ProductFeedback { get; set; }

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

        public IActionResult OnPostSubmitFeedback(int productId, int orderId)
        {
            if (ModelState.IsValid)
            {
                var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
                if (accountIdClaim != null && short.TryParse(accountIdClaim.Value, out short accountId))
                {
                    var productFeedBack = _productFeedbackService.GetProductFeedbackByProductAndOrderAndAccount(productId, orderId, accountId);
                    if (productFeedBack != null)
                    {
                        ListOrder = _orderService.GetAllOrderByAccount(accountId);

                        foreach (var order in ListOrder)
                        {
                            foreach (var item in order.OrderDetails)
                            {
                                item.Product = _productService.GetProduct(item.ProductId);
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Bạn đã đánh giá sản phẩm này rồi");
                        return Page();

                    }

                    ProductFeedback.AccountId = accountId;
                    ProductFeedback.ProductId = productId;
                    ProductFeedback.OrderId = orderId;
                    ProductFeedback.CreateDate = DateTime.UtcNow;
                    _productFeedbackService.CreateProductFeedback(ProductFeedback);
                }
                return RedirectToPage("/Orders/GetOrders");
            }
            else { 
                return RedirectToPage("/Orders/GetOrders"); 
            }
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            var order = _orderService.GetAllOrder().FirstOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                return RedirectToPage("/Orders/GetOrders");
            }

            foreach(var item in order.OrderDetails)
            {
                _productService.AddQuantityProduct(item.ProductId, item.Quantity, order.CreatedDate);
            }
            _orderService.UpdateOrderCancel(orderId);
            return RedirectToPage("/Orders/GetOrders");
        }
    }
}
