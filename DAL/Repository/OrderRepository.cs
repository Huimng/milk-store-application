using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public interface IOrderRepository
    {
        public Order CreateOrder(Order order);
        public List<Order> GetAllOrder();
        public List<Order> GetAllOrderByAccount(int idAccount);
        public void UpdateOrder(Order order);
        public void UpdateOrderCancel(int orderId);
        public List<Order> GetStatics();
    }
    public class OrderRepository : IOrderRepository
    {
        public Order CreateOrder(Order order)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<Order>().Add(order);
                    context.SaveChanges();
                    
                }
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                using (var context = new BSADBContext())
                { 
                    Order orderOld = context.Set<Order>().Include(x => x.OrderContact).FirstOrDefault(x=>x.OrderId == order.OrderId);
                    if(orderOld != null)
                    {
                        orderOld.OrderContact.CustomerName = order.OrderContact.CustomerName;
                        orderOld.Address = order.Address;
                        orderOld.OrderContact.Phone = order.OrderContact.Phone;
                        orderOld.Status = order.Status;
                        context.Set<Order>().Update(orderOld);
                        context.SaveChanges();
                    }

                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrderCancel(int orderId)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    Order orderOld = context.Set<Order>().Include(x => x.OrderContact).FirstOrDefault(x => x.OrderId == orderId);
                    if (orderOld != null)
                    {                      
                        orderOld.Status = OrderStatus.Canceled;
                        context.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<Order> GetAllOrder()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Order>().Include(na => na.OrderDetails).Include(x => x.OrderContact).OrderByDescending(n => n.CreatedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<Order> GetStatics()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Order>().Where(x=>x.Status== OrderStatus.Succeeded).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Order> GetAllOrderByAccount(int idAccount)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Order>().Where(x=>x.AccountId == idAccount).Include(na=>na.OrderDetails).Include(x=>x.OrderContact).OrderByDescending(n => n.CreatedDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

