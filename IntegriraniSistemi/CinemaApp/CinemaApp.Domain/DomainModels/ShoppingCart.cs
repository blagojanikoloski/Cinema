using CinemaApp.Domain.Identity;
using CinemaApp.Domain.Relations;
using System;
using System.Collections.Generic;


namespace CinemaApp.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual CinemaAppApplicationUser Owner { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }

    }
}
