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
    }
}
