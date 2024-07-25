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
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MilkStore.Pages.ProductManager
{
    [Authorize(Roles = "Staff")]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ProductLineService productLineService;

        public DetailsModel(IServiceProvider serviceProvider)
        {
            productLineService = new ProductLineService();
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }
        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public ProductLine ProductLine { get; set; }
        [BindProperty]
        public List<ProductLineSummary> ProductLineSummaries { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var product = _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
               List<ProductLineSummary> ProductLineSurima = _productService.GetAllExpireDate(id);
                
                foreach (var summary in ProductLineSurima)
                {
                    if(summary.IsDeleted == false)
                    {
                        summary.Quantity = 0;
                    }               
                   
                }
                ProductLineSummary[] productLinesArray = ProductLineSurima.ToArray();
                

                for (int i = 0; i < productLinesArray.Length -1; i++)
                {
                    for(int j = i+1; j < productLinesArray.Length; j++)
                    {
                        if (productLinesArray[i].AgeGroup.Equals(productLinesArray[j].AgeGroup) && productLinesArray[i].ExpireDate.Equals(productLinesArray[j].ExpireDate))
                        {
                            if (!productLinesArray[i].IsDeleted.Equals(productLinesArray[j].IsDeleted))
                            {
                                if (productLinesArray[i].Quantity == 0 || productLinesArray[j].Quantity == 0)
                                {
                                    if(productLinesArray[j].IsDeleted == false)
                                    {
                                        List<ProductLineSummary> sdafsa = productLinesArray.ToList();
                                        sdafsa.RemoveAt(j);
                                        productLinesArray = sdafsa.ToArray();
                                    }
                                    else
                                    {
                                        List<ProductLineSummary> sdafsa1 = productLinesArray.ToList();
                                        sdafsa1.RemoveAt(i);
                                        productLinesArray = sdafsa1.ToArray();
                                    }

                                   



                                }
                            }
                        }
                    }
                }

                ProductLineSummaries = productLinesArray.OrderBy(x=>x.ExpireDate).ToList();

               Product = product;
            }
            return Page();
        }

        public void RefreshPage(int id)
        {
            var product = _productService.GetProduct(id);
            List<ProductLineSummary> ProductLineSurima = _productService.GetAllExpireDate(id);

            foreach (var summary in ProductLineSurima)
            {
                if (summary.IsDeleted == false)
                {
                    summary.Quantity = 0;
                }

            }
            ProductLineSummary[] productLinesArray = ProductLineSurima.ToArray();


            for (int i = 0; i < productLinesArray.Length - 1; i++)
            {
                for (int j = i + 1; j < productLinesArray.Length; j++)
                {
                    if (productLinesArray[i].AgeGroup.Equals(productLinesArray[j].AgeGroup) && productLinesArray[i].ExpireDate.Equals(productLinesArray[j].ExpireDate))
                    {
                        if (!productLinesArray[i].IsDeleted.Equals(productLinesArray[j].IsDeleted))
                        {
                            if (productLinesArray[i].Quantity == 0 || productLinesArray[j].Quantity == 0)
                            {
                                if (productLinesArray[j].IsDeleted == false)
                                {
                                    List<ProductLineSummary> sdafsa = productLinesArray.ToList();
                                    sdafsa.RemoveAt(j);
                                    productLinesArray = sdafsa.ToArray();
                                }
                                else
                                {
                                    List<ProductLineSummary> sdafsa1 = productLinesArray.ToList();
                                    sdafsa1.RemoveAt(i);
                                    productLinesArray = sdafsa1.ToArray();
                                }





                            }
                        }
                    }
                }
            }

            ProductLineSummaries = productLinesArray.OrderBy(x => x.ExpireDate).ToList();

            Product = product;
        }

        public async Task<IActionResult> OnPostAsync(int productId) {
            if (ProductLine == null)
            {
                RefreshPage(productId);
                return Page();
            }
            if(ProductLine.ExpireDate <= DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Invalid Expire Date (Date must large today).");
                RefreshPage(productId);
                return Page();
            }
            if (string.IsNullOrEmpty(ProductLine.AgeGroup))
            {
                ModelState.AddModelError(string.Empty, "Invalid Age Group");
                RefreshPage(productId);
                return Page();
            }
            if (Quantity < 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Quantity.");
                RefreshPage(productId);
                return Page();
            }
            _productService.UpdateQuantityFromProductLine(productId, Quantity);
            for(int i = 0; i < Quantity; i++)
            {
                ProductLine productLine = new ProductLine();
                productLine.ProductId = productId;
                productLine.ExpireDate = DateTime.SpecifyKind(ProductLine.ExpireDate, DateTimeKind.Utc).ToUniversalTime();
                productLine.AgeGroup = ProductLine.AgeGroup;
                productLine.DeleteDate = DateTime.SpecifyKind(ProductLine.ExpireDate, DateTimeKind.Utc).ToUniversalTime();
                productLineService.AddProductLIne(productLine);

            }
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                List<ProductLineSummary> ProductLineSurima = _productService.GetAllExpireDate(productId);

                foreach (var summary in ProductLineSurima)
                {
                    if (summary.IsDeleted == false)
                    {
                        summary.Quantity = 0;
                    }

                }
                ProductLineSummary[] productLinesArray = ProductLineSurima.ToArray();

                for (int i = 0; i < productLinesArray.Length - 1; i++)
                {
                    for (int j = i + 1; j < productLinesArray.Length; j++)
                    {
                        if (productLinesArray[i].AgeGroup.Equals(productLinesArray[j].AgeGroup) && productLinesArray[i].ExpireDate.Equals(productLinesArray[j].ExpireDate))
                        {
                            if (!productLinesArray[i].IsDeleted.Equals(productLinesArray[j].IsDeleted))
                            {
                                if (productLinesArray[i].Quantity == 0 || productLinesArray[j].Quantity == 0)
                                {
                                    if (productLinesArray[j].IsDeleted == false)
                                    {
                                        List<ProductLineSummary> sdafsa = productLinesArray.ToList();
                                        sdafsa.RemoveAt(j);
                                        productLinesArray = sdafsa.ToArray();
                                    }
                                    else
                                    {
                                        List<ProductLineSummary> sdafsa1 = productLinesArray.ToList();
                                        sdafsa1.RemoveAt(i);
                                        productLinesArray = sdafsa1.ToArray();
                                    }

                                }
                            }
                        }
                    }
                }

                ProductLineSummaries = productLinesArray.ToList();
                Product = product;
            }
            return Page();

        }
    }
}
