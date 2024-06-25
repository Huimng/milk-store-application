using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IOrderContactRepository
    {
        public void CreateOrderContact(OrderContact orderContact);
        public List<OrderContact> GetAllOrderContact();
        public OrderContact GetOrderContact(int id);
    }
    public class OrderContactRepository : IOrderContactRepository
    {
        public void CreateOrderContact(OrderContact orderContact)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    context.Set<OrderContact>().Add(orderContact);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderContact> GetAllOrderContact()
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<OrderContact>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public OrderContact GetOrderContact(int id)
        {
            try
            {
                using (var context = new BSADBContext())
                {
                    return context.Set<OrderContact>().Where(x => x.OrdContacId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
