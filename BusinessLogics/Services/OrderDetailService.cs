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
    public interface IOrderDetailService
    {
        public void CreateOrderDetail(OrderDetail orderDetail);
        public List<OrderDetail> GetAllOrderDetail();
        public List<OrderDetail> GetAllOrderDetailByOrder(int idOrder);

    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IServiceProvider serviceProvider)
        {
            _orderDetailRepository = serviceProvider.GetRequiredService<IOrderDetailRepository>();
        }

        public void CreateOrderDetail(OrderDetail orderDetail) => _orderDetailRepository.CreateOrderDetail(orderDetail);
        public List<OrderDetail> GetAllOrderDetail() => _orderDetailRepository.GetAllOrderDetail();

        public List<OrderDetail> GetAllOrderDetailByOrder(int idOrder) => _orderDetailRepository.GetAllOrderDetailByOrder(idOrder);

    }
}
