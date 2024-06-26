using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IOrderDetailRepository
    {
        public void CreateOrderDetail(OrderDetail orderDetail);
        public List<OrderDetail> GetAllOrderDetail();
        public List<OrderDetail> GetAllOrderDetailByOrder(int idOrder);

    }


    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<OrderDetail>().Add(orderDetail);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderDetail> GetAllOrderDetail()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<OrderDetail>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<OrderDetail> GetAllOrderDetailByOrder(int idOrder)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<OrderDetail>().Where(x => x.OrderId == idOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
