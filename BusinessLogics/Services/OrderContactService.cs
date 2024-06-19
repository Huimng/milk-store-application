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
    public interface IOrderContactService
    {
        public void CreateOrderContact(OrderContact orderContact);
    }
    public class OrderContactService : IOrderContactService
    {
        private readonly  IOrderContactRepository _orderContactRepository;

        public OrderContactService(IServiceProvider serviceProvider)
        {
            _orderContactRepository = serviceProvider.GetRequiredService<IOrderContactRepository>();
        }

        public void CreateOrderContact(OrderContact orderContact) => _orderContactRepository.CreateOrderContact(orderContact);




    }
}
