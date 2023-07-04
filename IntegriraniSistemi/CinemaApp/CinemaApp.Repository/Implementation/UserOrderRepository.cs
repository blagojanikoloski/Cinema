using CinemaApp.Domain;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaApp.Repository.Implementation
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public UserOrderRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.ProductInOrders)
                .Include("ProductInOrders.Product")
                .ToListAsync().Result;
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.ProductInOrders)
               .Include("ProductInOrders.Product")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }

        public void AddUserOrder(UserOrder userOrder)
        {
            _context.UserOrder.Add(userOrder);
            _context.SaveChanges();
        }

        public List<UserOrder> GetUserOrdersByUserId(string userId)
        {
            return _context.UserOrder
                .Where(uo => uo.AppUserID == userId)
                .ToList();
        }

        public decimal GetTotalAmountByOrderId(int orderId)
        {
            return _context.UserOrder
                .Where(o => o.UserOrderID == orderId)
                .Select(o => o.TotalAmount)
                .FirstOrDefault();
        }

        public int GetLatestOrderID(string userID)
        {
            int orderID = _context.UserOrder
                .Where(o => o.AppUserID == userID)
                .OrderByDescending(o => o.UserOrderID)
                .Select(o => o.UserOrderID)
                .FirstOrDefault();

            return orderID;
        }
    }
}
