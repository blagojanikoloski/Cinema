using CinemaApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Implementation
{
    public class CartsRepository : ICartsRepository
    {
        private readonly ApplicationDbContext _context;

        public CartsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCartIdByUserId(string userId)
        {
            var cartId = await _context.Cart
                .Where(c => c.AppUserID == userId)
                .Select(c => c.CartID)
                .FirstOrDefaultAsync();

            return cartId;
        }
    }
}