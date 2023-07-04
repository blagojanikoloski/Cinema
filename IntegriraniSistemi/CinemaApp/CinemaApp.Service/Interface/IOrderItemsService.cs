using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IOrderItemsService
    {
        List<OrderItem> GetOrderItemsByOrderId(int orderId);

        List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> GetOrderItemsDetails(int orderId);
    }
}
