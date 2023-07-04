using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Relations;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Implementation
{
    public class CartsService : ICartsService
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly ICartItemsRepository _cartItemsRepository;
        private readonly IMoviesRepository _moviesRepository;
        private readonly IMovieDatesRepository _movieDatesRepository;
        private readonly IMovieDatesService _movieDatesService;

        public CartsService(ICartsRepository cartsRepository,ICartItemsRepository cartItemsRepository, IMovieDatesRepository movieDatesRepository, IMoviesRepository moviesRepository, IMovieDatesService movieDatesService)
        {
            _cartsRepository = cartsRepository;
            _cartItemsRepository = cartItemsRepository;
            _movieDatesRepository = movieDatesRepository;
            _moviesRepository = moviesRepository;
            _movieDatesService = movieDatesService;
        }

        public async Task<int> GetCartIdByUserId(string userId)
        {
            return await _cartsRepository.GetCartIdByUserId(userId);
        }

        public int CalculateCartTotal(int cartId)
        {

            int cartTotal = 0;
            List<CartItem> cartItems=_cartItemsRepository.GetCartItemsByCartId(cartId);

            foreach (CartItem cartItem in cartItems)
            {
                // Retrieve the MovieDatesID and Quantity from the cartItem
                int movieDatesID = cartItem.MovieDatesID;
                int quantity = cartItem.Quantity;

                // Retrieve the MovieDates.Price for the given MovieDatesID
                int price = _movieDatesService.GetMovieDatesPriceById(movieDatesID);

                // Calculate the subtotal for the current cartItem
                int subtotal = price * quantity;

                // Add the subtotal to the cartTotal
                cartTotal += subtotal;
            }
            return cartTotal;

        }

    }
}
