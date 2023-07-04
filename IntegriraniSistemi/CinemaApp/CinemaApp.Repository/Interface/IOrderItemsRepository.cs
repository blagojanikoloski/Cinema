using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Repository.Interface
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetOrderItemsByOrderId(int orderId);

        public List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> GetOrderItemsDetails(int orderId);
    }
}
