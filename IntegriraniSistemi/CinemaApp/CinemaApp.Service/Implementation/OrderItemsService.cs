using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Implementation
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrderItemsService(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _orderItemsRepository.GetOrderItemsByOrderId(orderId);
        }


         public List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> GetOrderItemsDetails(int orderId)
        {
            return _orderItemsRepository.GetOrderItemsDetails(orderId);
        }

    }
}
