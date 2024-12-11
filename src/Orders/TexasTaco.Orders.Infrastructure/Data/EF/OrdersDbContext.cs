using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Persistence.AccountCreatedInboxMessages;
using TexasTaco.Orders.Persistence.ProductPriceChangedInbox;
using TexasTaco.Orders.Persistence.UserUpdatedInboxMessages;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : DbContext(options)
    {
        public DbSet<CartProduct> CartProducts { get; private set; }
        public DbSet<Cart> Carts { get; private set; }
        public DbSet<CheckoutCart> CheckoutCarts { get; private set; }
        public DbSet<Customer> Customers { get; private set; }
        public DbSet<AccountCreatedInboxMessage> AccountCreatedInboxMessages { get; private set; }
        public DbSet<UserUpdatedInboxMessage> UserUpdatedInboxMessages { get; private set; }
        public DbSet<ProductPriceChangedInboxMessage> ProductPriceChangedInboxMessages { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
