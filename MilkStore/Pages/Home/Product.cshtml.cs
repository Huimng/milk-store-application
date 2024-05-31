using BusinessLogics.Services;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MockQueryable;

namespace MilkStore.Pages.Home
{
    public class ProductModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductModel(IServiceProvider serviceProvider)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }

        public IList<Product> Products { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public async Task OnGetAsync(int pageIndex = 1)
        {
            int pageSize = 5;
            var productsQuery = _productService.GetAllProduct();

            // Get total count of products
            var count = await productsQuery.CountAsync();

            // Calculate total pages
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Get products for the current page
            Products = await productsQuery.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            PageIndex = pageIndex;
        }
    }
}
