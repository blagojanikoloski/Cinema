using CinemaApp.Domain.Identity;
using CinemaApp.Domain.Relations;
using System;
using System.Collections.Generic;

namespace CinemaApp.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public CinemaAppApplicationUser User { get; set; }

        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
