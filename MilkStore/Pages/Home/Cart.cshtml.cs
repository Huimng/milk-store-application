using BusinessLogics.Helpers;
using BusinessObjects.Models;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogics.Services;

namespace MilkStore.Pages.Home
{
    public class CartModel : PageModel
    {
        private readonly IProductService _productService;

        public CartModel(IServiceProvider serviceProvider)
        {
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        public List<CartItem> Cart { get; set; }
        public IActionResult OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return Page();
        }

        public IActionResult OnPostAddToCart(int productId, int quantity)
        {
            Product product = _productService.GetProduct(productId);
            // Fetch the product details from your data source
            var productToCart = new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Discount,
                Quantity = quantity,     //quantity of cart
                UrlImage = product.UrlImage
            };

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.Find(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(productToCart);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Redirect($"/Home/ProductDetail?proId={productId}");
        }

        public IActionResult OnPostDelete(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToPage();
        }
    }
}
