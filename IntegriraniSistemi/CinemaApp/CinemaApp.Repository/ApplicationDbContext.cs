using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.Identity;
using CinemaApp.Domain.Relations;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CinemaApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<CinemaAppApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCards { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieDates> MovieDates { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<UserOrder> UserOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Cart>()
                .Property(z => z.CartID)
                .ValueGeneratedOnAdd();


            builder.Entity<ShoppingCart>()
                .HasOne<CinemaAppApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<ProductInOrder>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.OrderId);
        }
    }
}
