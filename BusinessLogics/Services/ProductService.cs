using BusinessObjects;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IProductService
    {
        public IQueryable<Product> GetAllProduct();
        public Product GetProduct(int id);
    }
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IServiceProvider serviceProvider)
        {
            _productRepository = serviceProvider.GetRequiredService<IProductRepository>();
        }

        public IQueryable<Product> GetAllProduct() => _productRepository.GetAllProduct();
        public Product GetProduct(int id) => _productRepository.GetProduct(id);

    }
}
