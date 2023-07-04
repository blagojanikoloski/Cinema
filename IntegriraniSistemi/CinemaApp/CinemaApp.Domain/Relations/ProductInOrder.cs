﻿using CinemaApp.Domain.DomainModels;
using System;

namespace CinemaApp.Domain.Relations
{
    public class ProductInOrder : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
