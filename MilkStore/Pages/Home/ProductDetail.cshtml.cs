using BusinessLogics.Helpers;
using BusinessLogics.Services;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkStore.Pages.Home
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductFeedbackService _productFeedbackService;
        private readonly IAccountService _accountService;

        public ProductDetailModel(IServiceProvider serviceProvider)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _productFeedbackService = serviceProvider.GetRequiredService<IProductFeedbackService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
        }
        [BindProperty]
        public List<ProductFeedback> ListProductFeedback { get; set; }
        public void OnGet(int proId)
        {
            Product product = _productService.GetProduct(proId);
            ViewData["product"] = product;
            ListProductFeedback = _productFeedbackService.GetAllProductFeedbackByProduct(proId);
            foreach (var feedback in ListProductFeedback)
            {
                feedback.Account = _accountService.GetAccount(feedback.AccountId);
            }
        }


    }
}
