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
                    return context.Set<Product>().ToList();
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
    }
}
