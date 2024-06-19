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
    public interface IOrderService
    {
        public void CreateOrder(Order order);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IServiceProvider serviceProvider)
        {
            _orderRepository= serviceProvider.GetRequiredService<IOrderRepository>();
        }

        public void CreateOrder(Order order) => _orderRepository.CreateOrder(order);
    }
}
