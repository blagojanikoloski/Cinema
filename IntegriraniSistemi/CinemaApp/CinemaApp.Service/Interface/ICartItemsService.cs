using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Interface
{
    public interface ICartItemsService
    {
        void AddCartItem(int cartId, int movieId, int quantity, int movieDatesId);

        List<CartItem> GetCartItemsByCartId(int cartId);

        List<(int CartItemID, string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> GetCartItemDetails(int cartId);

        void UpdateCartItemQuantity(CartItem cartItem, string action);

        Task<CartItem> GetCartItemAsync(int cartItemID);

        void DeleteCartItemsByCartID(int cartID);
    }
}
