using CinemaApp.Domain;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Service.Implementation
{
    public class UserOrderService : IUserOrderService
    {
        
        private readonly IUserOrderRepository _userOrderRepository;
        private readonly IMovieDatesService _movieDatesService;
        private readonly ApplicationDbContext _context;


        public UserOrderService(IUserOrderRepository userOrderRepository, IMovieDatesService movieDatesService,ApplicationDbContext context)
        {
            _userOrderRepository = userOrderRepository;
            _movieDatesService = movieDatesService;
            _context = context;
        }
        public void AddUserOrder(string appUserId, DateTime date, decimal totalAmount,List<CartItem> cartItems)
        {
            UserOrder userOrder = new UserOrder
            {
                AppUserID = appUserId,
                Date = date,
                TotalAmount = totalAmount
            };

            _userOrderRepository.AddUserOrder(userOrder);


            foreach (CartItem cartItem in cartItems)
            {
                // Retrieve the MovieDates for the given MovieDatesID
                MovieDates movieDates = _movieDatesService.GetMovieDatesByID(cartItem.MovieDatesID);

                int pricePerTicket = movieDates.Price;

                OrderItem orderItem = new OrderItem
                {
                    UserOrderID = userOrder.UserOrderID, // Set the UserOrderID from the newly created UserOrder
                    MovieDatesID = cartItem.MovieDatesID,
                    Quantity = cartItem.Quantity,
                    Price = pricePerTicket
                };

                _context.OrderItem.Add(orderItem);
            }
        }

        public List<UserOrder> GetUserOrdersByUserId(string userId)
        {
            return _userOrderRepository.GetUserOrdersByUserId(userId);
        }

        public decimal GetTotalAmountByOrderId(int orderId)
        {
            return _userOrderRepository.GetTotalAmountByOrderId(orderId);
        }

        public int GetLatestOrderID(string userID)
        {
            return _userOrderRepository.GetLatestOrderID(userID);
        }
    }
}
