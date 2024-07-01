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
        public List<Product> GetAllProduct();
        public Product GetProduct(int id);
        public void UpdateProduct(Product product);
        public void CreateProduct(Product product);
        public void DeleteProduct(int id);
        public List<Product> GetAllProductStaff();
        public void UpdateQuantityProduct(int idProduct, int quantity);
    }
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IServiceProvider serviceProvider)
        {
            _productRepository = serviceProvider.GetRequiredService<IProductRepository>();
        }

        public List<Product> GetAllProduct() => _productRepository.GetAllProduct();
        public Product GetProduct(int id) => _productRepository.GetProduct(id);
        public void UpdateProduct(Product product) => _productRepository.UpdateProduct(product);
        public void CreateProduct(Product product) => _productRepository.CreateProduct(product);
        public void DeleteProduct(int id) => _productRepository?.DeleteProduct(id);
        public List<Product> GetAllProductStaff() => _productRepository.GetAllProductStaff();
        public void UpdateQuantityProduct(int idProduct, int quantity) => _productRepository.UpdateQuantityProduct(idProduct, quantity);
    }
}
