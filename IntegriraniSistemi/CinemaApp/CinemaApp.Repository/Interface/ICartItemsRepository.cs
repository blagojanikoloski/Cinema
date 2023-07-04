using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Interface
{
    public interface ICartItemsRepository
    {
        void AddCartItem(CartItem cartItem);

        List<CartItem> GetCartItemsByCartId(int cartId);


        List<(int CartItemID, string MovieName, int Quantity, DateTime MovieDate, string Genre, int Price)> GetCartItemDetails(int cartId);

        void IncreaseCartItemQuantity(CartItem cartItem);
        void DecreaseCartItemQuantity(CartItem cartItem);

        Task<CartItem> GetCartItemByIdAsync(int cartItemID);

        void DeleteByCartID(int cartID);

    }
}
