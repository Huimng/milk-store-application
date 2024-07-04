using BusinessObjects;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Services
{
    public interface IProductLineService
    {
        public void AddProductLIne(ProductLine p);
        public List<ProductLine> GetProductLines();
        public ProductLine Get(int id);
        public void Update(ProductLine p);
        public void Delete(int id);
    }
    public class ProductLineService : IProductLineService
    {
        public ProductLineRepository productLineRepository;
        public ProductLineService()
        {
            productLineRepository = new ProductLineRepository();
        }
        public void AddProductLIne(ProductLine p) => productLineRepository.CreateProductLine(p);

        public void Delete(int id) => productLineRepository.DeleteProductLine(id);

        public ProductLine? Get(int id) => productLineRepository.GetProductLineById(id);

        public List<ProductLine> GetProductLines() => productLineRepository.GetAllProductLine();

        public void Update(ProductLine p) => productLineRepository.UpdateProductLine(p);
    }
}
