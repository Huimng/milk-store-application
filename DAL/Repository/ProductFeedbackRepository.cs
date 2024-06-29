using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IProductFeedbackRepository
    {
        public void CreateProductFeedback(ProductFeedback productFeedback);
        public List<ProductFeedback> GetAllProductFeedbackByProduct(int idProduct);
        public ProductFeedback GetProductFeedbackByProductAndOrder(int idProduct, int idOrder, int idAccount);
    }
    public class ProductFeedbackRepository : IProductFeedbackRepository
    {
        public void CreateProductFeedback(ProductFeedback productFeedback)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<ProductFeedback>().Add(productFeedback);
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ProductFeedback> GetAllProductFeedbackByProduct(int idProduct)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<ProductFeedback>().Where(x => x.ProductId == idProduct).OrderByDescending(x=>x.CreateDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ProductFeedback GetProductFeedbackByProductAndOrder(int idProduct, int idOrder, int idAccount)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<ProductFeedback>().Where(x => x.ProductId == idProduct && x.OrderId == idOrder && x.AccountId == idAccount).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
