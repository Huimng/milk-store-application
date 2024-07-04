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
    public class IndexModel : PageModel
    {
        private readonly ProductLineService productLineService;

        public IndexModel()
        {
            productLineService = new ProductLineService();
        }

        public IList<ProductLine> ProductLine { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (productLineService != null)
            {
                ProductLine = productLineService.GetProductLines();
                
            }
        }
    }
}
