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
        public Order CreateOrder(Order order);
        public List<Order> GetAllOrder();
        public List<Order> GetAllOrderByAccount(int idAccount);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IServiceProvider serviceProvider)
        {
            _orderRepository= serviceProvider.GetRequiredService<IOrderRepository>();
        }

        public Order CreateOrder(Order order) => _orderRepository.CreateOrder(order);
        public List<Order> GetAllOrder() => _orderRepository.GetAllOrder();
        public List<Order> GetAllOrderByAccount(int idAccount) => _orderRepository.GetAllOrderByAccount(idAccount);
    }
}
