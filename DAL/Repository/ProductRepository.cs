using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void UpdateQuantityProduct(int idProduct, int quantity);
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

        public Product GetProduct(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    var _product = context.Set<Product>().FirstOrDefault(x => x.ProductId == id);

                    return _product;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                        productOld.Quantity = product.Quantity;
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

        public void UpdateQuantityProduct(int idProduct, int quantity)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Product product = context.Set<Product>().FirstOrDefault(x => x.ProductId == idProduct);
                    if (product != null)
                    {
                        product.Quantity = product.Quantity - quantity;
                        if(product.Quantity == 0)
                        {
                            product.Status = ProductStatus.OutOfStock;
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
