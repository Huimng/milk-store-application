using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.Models;

namespace MilkStore.Pages.ProductManager
{
    [Authorize(Roles = "Staff")]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ProductLineService productLineService;

        public DetailsModel(IServiceProvider serviceProvider)
        {
            productLineService = new ProductLineService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public ProductLine ProductLine { get; set; }
        [BindProperty]
        public List<ProductLineSummary> ProductLineSummaries { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var product = _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                ProductLineSummaries = _productService.GetAllExpireDate(id);
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int productId) {
            if (ProductLine == null)
            {
                return Page();
            }
            if (Quantity < 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Quantity.");
                return Page();
            }
            _productService.UpdateQuantityFromProductLine(productId, Quantity);
            for(int i = 0; i < Quantity; i++)
            {
                ProductLine productLine = new ProductLine();
                productLine.ProductId = productId;
                productLine.ExpireDate = DateTime.SpecifyKind(ProductLine.ExpireDate, DateTimeKind.Utc);
                productLine.AgeGroup = ProductLine.AgeGroup;
                productLineService.AddProductLIne(productLine);

            }
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                ProductLineSummaries = _productService.GetAllExpireDate(productId);
                Product = product;
            }
            return Page();

        }
    }
}
