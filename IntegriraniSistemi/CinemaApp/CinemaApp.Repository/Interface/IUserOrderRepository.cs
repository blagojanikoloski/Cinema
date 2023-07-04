using CinemaApp.Domain;
using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Repository.Interface
{
    public interface IUserOrderRepository
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(BaseEntity model);

        void AddUserOrder(UserOrder userOrder);

        List<UserOrder> GetUserOrdersByUserId(string userId);

        decimal GetTotalAmountByOrderId(int orderId);

        int GetLatestOrderID(string userID);
    }
}
