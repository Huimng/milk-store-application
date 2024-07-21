using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IProductRepository
    {
        public List<Product> GetAllProduct();
        public Product GetProduct(int id);
        public void CreateProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
        public List<Product> GetAllProductStaff();
        public void UpdateQuantityProduct(int idProduct, int quantity, DateTime c);
        public void UpdateQuantityFromProductLine(int idProduct, int quantity);
        public List<ProductLineSummary> GetAllExpireDate(int idProduct);
        public void AddQuantityProduct(int idProduct, int quantity, DateTime c);
        public Product GetProductById(int id);
    }
    public class ProductRepository : IProductRepository
    {
        //private readonly BSADBContext _context;

        //public ProductRepository(BSADBContext context)
        //{
        //    _context = context;
        //}

        public List<Product> GetAllProduct()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Product>().Where(x => x.Status == ProductStatus.Available || x.Status == ProductStatus.OutOfStock)
                    .OrderByDescending(x=>x.CreatedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Product> GetAllProductStaff()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Product>()
                    .OrderByDescending(x => x.CreatedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ProductLineSummary> GetAllExpireDate(int idProduct)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                var result = context.Set<ProductLine>()
               .Where(pl => pl.ProductId == idProduct)
               .OrderBy(pl=>pl.IsActived)
               .GroupBy(pl => new { pl.ExpireDate, pl.AgeGroup, pl.IsActived })              
               .Select(g => new ProductLineSummary
               {
                   Quantity = g.Count(),
                   ExpireDate = g.Key.ExpireDate,
                   AgeGroup = g.Key.AgeGroup,
                   IsDeleted = g.Key.IsActived
               })
               .ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Product GetProduct(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var _product = context.Set<Product>().Include(x => x.ProductLines).FirstOrDefault(x => x.ProductId == id);

                    return _product;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Product GetProductById(int id)
        {

            using (var context = new BSADBContext())
            {
                var product = context.Set<Product>().FirstOrDefault(x => x.ProductId == id);

                return product;
            }

        }

        public void CreateProduct(Product product)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var lastproduct = context.Set<Product>().OrderByDescending(t => t.ProductId).FirstOrDefault();
                    if (lastproduct != null)
                    {
                        product.ProductId = lastproduct.ProductId + 1;
                        context.Set<Product>().Add(product);
                        context.SaveChanges();
                    }
                    else
                    {
                        product.ProductId = 1;
                        context.Set<Product>().Add(product);
                        context.SaveChanges();
                    }                   
                }
            }
            catch(Exception ex) 
            { 
            throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Product productOld = context.Set<Product>().FirstOrDefault(x => x.ProductId == product.ProductId);
                    if(productOld != null)
                    {
                        productOld.ProductName = product.ProductName;
                        productOld.ProductCode = product.ProductCode;
                        productOld.Description = product.Description;
                        productOld.Brand = product.Brand;
                        productOld.UrlImage = product.UrlImage;
                        productOld.Status = product.Status;
                        productOld.Discount = product.Discount;
                        context.SaveChanges();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteProduct(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Product product = context.Set<Product>().FirstOrDefault(x => x.ProductId == id);
                    if(product != null)
                    {
                        product.Status = ProductStatus.Deleted;
                        context.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateQuantityProduct(int idProduct, int quantity, DateTime createorder)
        {
            using (var context = new BSADBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = context.Set<Product>().Include(p => p.ProductLines).FirstOrDefault(p => p.ProductId == idProduct);
                        if (product == null)
                        {
                            throw new InvalidOperationException($"Product with ID {idProduct} not found.");
                        }

                        if (product.Quantity < quantity)
                        {
                            throw new InvalidOperationException("Insufficient product quantity.");
                        }

                        product.Quantity -= quantity;
                        if (product.Quantity == 0)
                        {
                            product.Status = ProductStatus.OutOfStock;
                        }

                        var productLinesToRemove = context.Set<ProductLine>()
                            .Where(pl => pl.ProductId == idProduct && pl.IsActived == true )
                            .OrderBy(pl => pl.ExpireDate)
                            .Take(quantity)
                            .ToList();

                        if (productLinesToRemove.Count < quantity)
                        {
                            throw new InvalidOperationException("Insufficient product lines available.");
                        }

                        foreach(var line in productLinesToRemove)
                        {
                            line.IsActived = false;
                            line.DeleteDate = createorder;
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public void AddQuantityProduct(int idProduct, int quantity, DateTime createorder)
        {
            using (var context = new BSADBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = context.Set<Product>().Include(p => p.ProductLines).FirstOrDefault(p => p.ProductId == idProduct);
                        if (product == null)
                        {
                            throw new InvalidOperationException($"Product with ID {idProduct} not found.");
                        }

                        product.Quantity += quantity;
                        if (product.Quantity != 0)
                        {
                            product.Status = ProductStatus.Available;
                        }

                        var productLinesToAdd = context.Set<ProductLine>()
                            .Where(pl => pl.ProductId == idProduct && pl.IsActived == false)
                            .OrderByDescending(pl => pl.ExpireDate)
                            .ToList();

                        if (productLinesToAdd.Count < quantity)
                        {
                            throw new InvalidOperationException("Insufficient product lines available.");
                        }

                        foreach (var line in productLinesToAdd)
                        {
                            if(line.DeleteDate == createorder)
                            {
                                line.IsActived = true;
                            }
                            
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public void UpdateQuantityFromProductLine(int idProduct, int quantity)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Product product = context.Set<Product>().FirstOrDefault(x => x.ProductId == idProduct);
                    if (product != null)
                    {
                        product.Quantity = product.Quantity + quantity;
                        if(product.Quantity != 0)
                        {
                            product.Status = ProductStatus.Available;
                        }
                        context.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
