using BusinessObjects;
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
        public void CreateOrder(Order order);
    }
    public class OrderRepository : IOrderRepository
    {
        public void CreateOrder(Order order)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<Order>().Add(order);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

