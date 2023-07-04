using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Implementation
{
    public class CartItemsService : ICartItemsService
    {
        private readonly ICartItemsRepository _cartItemsRepository;
        private readonly IMoviesService _moviesService;
        private readonly IMovieDatesService _movieDatesService;
        private readonly ApplicationDbContext _context;

        public CartItemsService(ICartItemsRepository cartItemsRepository,IMoviesService moviesService,IMovieDatesService movieDatesService, ApplicationDbContext context)
        {
            _cartItemsRepository = cartItemsRepository;
            _moviesService = moviesService;
            _movieDatesService = movieDatesService;
            _context = context;
        }

        public void AddCartItem(int cartId, int movieId, int quantity, int movieDatesId)
        {
            CartItem cartItem = new CartItem
            {
                CartID = cartId,
                MovieID = movieId,
                Quantity = quantity,
                MovieDatesID = movieDatesId
            };

            _cartItemsRepository.AddCartItem(cartItem);
        }

        public List<CartItem> GetCartItemsByCartId(int cartId)
        {
            return _cartItemsRepository.GetCartItemsByCartId(cartId);
        }

        public List<(int CartItemID, string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> GetCartItemDetails(int cartId)
        {
            return _cartItemsRepository.GetCartItemDetails(cartId);
        }

        public void UpdateCartItemQuantity(CartItem cartItem, string action)
        {
            
            if (action == "add")
            {
                _cartItemsRepository.IncreaseCartItemQuantity(cartItem);
            }
            else if (action == "remove" && (cartItem.Quantity)>1)
            {
                _cartItemsRepository.DecreaseCartItemQuantity(cartItem);
                
            }
            else if(action == "remove" && (cartItem.Quantity) == 1)
            {
                _context.CartItem.Remove(cartItem);
            }

            _context.SaveChanges();
        }

        public async Task<CartItem> GetCartItemAsync(int cartItemID)
        {
            return await _cartItemsRepository.GetCartItemByIdAsync(cartItemID);
        }

        public void DeleteCartItemsByCartID(int cartID)
        {
            _cartItemsRepository.DeleteByCartID(cartID);
        }

    }
}
