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

namespace MilkStore.Pages.ProductLineManager
{
    public class DetailsModel : PageModel
    {
        private readonly ProductLineService productLineService;
        private readonly IProductService productService;

        public DetailsModel(IServiceProvider serviceProvider)
        {
            productLineService = new ProductLineService();
            productService = serviceProvider.GetRequiredService<IProductService>();
        }

      public ProductLine ProductLine { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || productLineService == null)
            {
                return NotFound();
            }

            var productline = productLineService.Get(id);
            if (productline == null)
            {
                return NotFound();
            }
            else 
            {
                ProductLine = productline;
                ProductLine.Product = productService.GetProduct(productline.ProductId);
            }
            return Page();
        }
    }
}
