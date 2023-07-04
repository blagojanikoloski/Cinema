using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Implementation
{
    public class CartItemsRepository : ICartItemsRepository
    {
        private readonly ApplicationDbContext _context;
 


        public CartItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddCartItem(CartItem cartItem)
        {
            _context.CartItem.Add(cartItem);
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItemsByCartId(int cartId)
        {
            return _context.CartItem
                .Where(ci => ci.CartID == cartId)
                .ToList();
        }


        public List<(int CartItemID, string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> GetCartItemDetails(int cartId)
        {

            List<CartItem> cartItems = GetCartItemsByCartId(cartId);

            List<(int CartItemID, string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> cartItemDetails =
                    cartItems.Select(ci => (
                        CartItemID: ci.CartItemID,
                        MovieName: _context.Movie
                            .Where(m => m.MovieID == ci.MovieID)
                            .Select(m => m.MovieName)
                            .FirstOrDefault(),
                        Quantity: ci.Quantity,
                        MovieDate: _context.MovieDates
                            .Where(md => md.MovieDatesID == ci.MovieDatesID)
                            .Select(md => md.Date)
                            .FirstOrDefault(),
                        Genre: _context.Movie
                            .Where(m => m.MovieID == ci.MovieID)
                            .Select(m => m.Genre)
                            .FirstOrDefault(),
                        Price: _context.MovieDates
                            .Where(md => md.MovieDatesID == ci.MovieDatesID)
                            .Select(md => md.Price)
                            .FirstOrDefault()
                    ))
                    .ToList();
            return cartItemDetails;
        }

        public void IncreaseCartItemQuantity(CartItem cartItem)
        {
            cartItem.Quantity++;
            _context.SaveChanges();
        }

        public void DecreaseCartItemQuantity(CartItem cartItem)
        {
            
             cartItem.Quantity--;
            _context.SaveChanges();
        }

       

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemID)
        {
            return await _context.CartItem.FindAsync(cartItemID);
        }

        public void DeleteByCartID(int cartID)
        {
            var cartItems = _context.CartItem.Where(ci => ci.CartID == cartID);
            _context.CartItem.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}
