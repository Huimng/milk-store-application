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
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 3; // Number of articles per page
        public int TotalPages { get; set; }


        public async Task OnGetAsync(int currentPage = 1)
        {
            CurrentPage = currentPage;
            var productsQuery = _productService.GetAllProduct();

            TotalPages = (int)Math.Ceiling(productsQuery.Count / (double)PageSize);

            Products = productsQuery
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
        }
    }
}
