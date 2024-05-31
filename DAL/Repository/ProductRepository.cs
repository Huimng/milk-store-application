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
        public IQueryable<Product> GetAllProduct();
        public Product GetProduct(int id);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly BSADBContext _context;

        public ProductRepository(BSADBContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetAllProduct()
        {
            try
            {
                return _context.Set<Product>().AsQueryable();
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
               var _product = _context.Set<Product>().FirstOrDefault(x => x.ProductId == id);
                return _product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
