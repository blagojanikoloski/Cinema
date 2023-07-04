using CinemaApp.Domain;
using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Interface
{
    public interface IUserOrderService
    {
  

        void AddUserOrder(string appUserId, DateTime date, decimal totalAmount, List<CartItem> cartItems);

        List<UserOrder> GetUserOrdersByUserId(string userId);

        decimal GetTotalAmountByOrderId(int orderId);

        int GetLatestOrderID(string userID);
    }
}
