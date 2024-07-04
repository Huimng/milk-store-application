using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DAL;
using BusinessLogics.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MilkStore.Pages.ProductLineManager
{
    public class EditModel : PageModel
    {
        private readonly ProductLineService productLineService;
        private readonly IProductService productService;

        public EditModel(IServiceProvider serviceProvider)
        { 
            productLineService = new ProductLineService();
            productService = serviceProvider.GetRequiredService<IProductService>();
        }

        [BindProperty]
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
            ProductLine = productline;
           ViewData["ProductId"] = new SelectList(productService.GetAllProduct(), "ProductId", "Brand");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                ProductLine.ExpireDate = DateTime.SpecifyKind(ProductLine.ExpireDate, DateTimeKind.Utc);
                productLineService.Update(ProductLine);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductLineExists(ProductLine.ProductLineId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductLineExists(int id)
        {
            return productService.GetProduct(id) != null;
        }


    }
}
