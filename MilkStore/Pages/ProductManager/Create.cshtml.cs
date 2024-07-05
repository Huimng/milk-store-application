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
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.Pages.ProductManager
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public IFormFile Photo { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (Product == null)
          {
                return Page();
          }
            if (Product.Discount < 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Price.");
                return Page();
            }

            if (Photo != null)
            {
                Product.UrlImage = ProcessUploadedFile();
            }
            Product.Quantity = 0;
            Product.CreatedDate = DateTime.UtcNow;
            _productService.CreateProduct(Product);

            return RedirectToPage("./Index");
        }
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder =
                Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
    }
}
