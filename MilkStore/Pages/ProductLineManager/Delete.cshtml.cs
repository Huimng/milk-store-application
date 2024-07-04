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
    public class DeleteModel : PageModel
    {
        private readonly ProductLineService productLineService;

        public DeleteModel()
        {
            productLineService = new ProductLineService();
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
            else 
            {
                ProductLine = productline;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null || productLineService == null)
            {
                return NotFound();
            }
            var productline = productLineService.Get(id);

            if (productline != null)
            {
                ProductLine = productline;
                productLineService.Delete(id);
                
            }

            return RedirectToPage("./Index");
        }
    }
}
