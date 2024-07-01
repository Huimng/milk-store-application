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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace MilkStore.Pages.ProductManager
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditModel(IServiceProvider serviceProvider,IWebHostEnvironment webHostEnvironment)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public IFormFile Photo { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            if (Product.Quantity < 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Quantity.");
                return Page();
            }
            if (Product.Discount < 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Price.");
                return Page();
            }
            if(Photo != null)
            {
                if (Product.UrlImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    "uploads", Product.UrlImage);
                    System.IO.File.Delete(filePath);
                }
                    Product.UrlImage = ProcessUploadedFile();
            }
            

            try
            {
               _productService.UpdateProduct(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
               return NotFound();
            }
            
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
