using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MilkStore.Pages.ProductLineManager
{
    public class CreateModel : PageModel
    {
        private readonly ProductLineService productLineService;
        private readonly IProductService productService;

        public CreateModel(IServiceProvider serviceProvider)
        {
            productLineService = new ProductLineService();
            productService = serviceProvider.GetRequiredService<IProductService>();
        }

        public IActionResult OnGet()
        {
        ViewData["ProductId"] = new SelectList(productService.GetAllProduct(), "ProductId", "Brand");
            return Page();
        }

        [BindProperty]
        public ProductLine ProductLine { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
                    var username = Request.Cookies["Username"];
          if (!ModelState.IsValid || productLineService == null || ProductLine == null)
            {
                return Page();
            }
            ProductLine.ExpireDate = DateTime.SpecifyKind(ProductLine.ExpireDate, DateTimeKind.Utc);
            productLineService.AddProductLIne(ProductLine);

            return RedirectToPage("./Index");
        }
    }
}
