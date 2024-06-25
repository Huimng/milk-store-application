using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IOrderRepository
    {
        public Order CreateOrder(Order order);
        public List<Order> GetAllOrder();
        public List<Order> GetAllOrderByAccount(int idAccount);

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

        public List<Order> GetAllOrder()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<Order>().ToList();
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
                    return context.Set<Order>().Where(x=>x.AccountId == idAccount).Include(na=>na.OrderDetails).Include(x=>x.OrderContact).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

