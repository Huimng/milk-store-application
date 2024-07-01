using BusinessLogics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkStore.Pages.Orders
{
    [Authorize(Roles = "Admin")]
    public class GetStaticsModel : PageModel
    {
        private readonly IOrderService _orderService;
        public GetStaticsModel(IServiceProvider serviceProvider)
        {
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
        }
        [BindProperty]
        public double TotalStatics { get; set; } = 0;
        public void OnGet()
        {
            var listOderStatic = _orderService.GetStatics();
            foreach (var item in listOderStatic)
            {
                TotalStatics = TotalStatics + item.GrandTotal;
            }
        }
    }
}
