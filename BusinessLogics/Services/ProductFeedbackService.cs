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
    public interface IProductFeedbackService
    {
        public void CreateProductFeedback(ProductFeedback productFeedback);
        public List<ProductFeedback> GetAllProductFeedbackByProduct(int idProduct);
        public ProductFeedback GetProductFeedbackByProductAndOrderAndAccount(int idProduct, int idOrder, int idAccount);
    }
    public class ProductFeedbackService : IProductFeedbackService
    {
        private readonly IProductFeedbackRepository _productFeedbackRepository;
        public ProductFeedbackService(IServiceProvider serviceProvider)
        {
            _productFeedbackRepository = serviceProvider.GetRequiredService<IProductFeedbackRepository>();
        }
        public void CreateProductFeedback(ProductFeedback productFeedback) => _productFeedbackRepository.CreateProductFeedback(productFeedback);
        public List<ProductFeedback> GetAllProductFeedbackByProduct(int idProduct) => _productFeedbackRepository.GetAllProductFeedbackByProduct(idProduct);
        public ProductFeedback GetProductFeedbackByProductAndOrderAndAccount(int idProduct, int idOrder, int idAccount) => _productFeedbackRepository.GetProductFeedbackByProductAndOrder(idProduct, idOrder, idAccount);
    }
}
