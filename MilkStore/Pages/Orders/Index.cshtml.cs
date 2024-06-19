using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkStore.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public List<CartItem> Cart { get; set; }
        [BindProperty]
        public OrderContact OrderContact { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        private readonly IOrderContactService _orderContactService;

        public IndexModel(IServiceProvider serviceProvider)
        {
            _orderContactService = serviceProvider.GetRequiredService<IOrderContactService>();
        }

        public IActionResult OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return Page();
        }


    }
}
