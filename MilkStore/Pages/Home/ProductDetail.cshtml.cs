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

        public ProductDetailModel(IServiceProvider serviceProvider)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        public void OnGet(int proId)
        {
            Product product = _productService.GetProduct(proId);
            ViewData["product"] = product;
        }

       
    }
}
