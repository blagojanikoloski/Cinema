using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Repository.Implementation
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _context.OrderItem
                .Where(oi => oi.UserOrderID == orderId)
                .ToList();
        }

        public List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> GetOrderItemsDetails(int orderId)
        {
            List<OrderItem> orderItems = GetOrderItemsByOrderId(orderId);

            List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> orderItemDetails =
                orderItems.Select(oi => (
                    OrderItemID: oi.OrderItemID,
                    MovieName: _context.Movie
                        .Where(m => m.MovieID == _context.MovieDates
                            .Where(md => md.MovieDatesID == oi.MovieDatesID)
                            .Select(md => md.MovieID)
                            .FirstOrDefault())
                        .Select(m => m.MovieName)
                        .FirstOrDefault(),
                    Quantity: oi.Quantity,
                    MovieDate: _context.MovieDates
                        .Where(md => md.MovieDatesID == oi.MovieDatesID)
                        .Select(md => md.Date)
                        .FirstOrDefault(),
                    Price: oi.Price
                ))
                .ToList();

            return orderItemDetails;
        }
    }
}
    

