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

namespace MilkStore.Pages.ProductManager
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IServiceProvider serviceProvider)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {           
            Product = _productService.GetAllProductStaff();
        }
    }
}
