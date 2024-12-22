using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : DbContext(options)
    {
        public DbSet<CartProduct> CartProducts { get; private set; }
        public DbSet<Cart> Carts { get; private set; }
        public DbSet<CheckoutCart> CheckoutCarts { get; private set; }
        public DbSet<Order> Orders { get; private set; }
        public DbSet<Customer> Customers { get; private set; }
        public DbSet<InboxMessage<AccountCreatedEventMessage>> AccountCreatedInboxMessages { get; private set; }
        public DbSet<InboxMessage<UserUpdatedEventMessage>> UserUpdatedInboxMessages { get; private set; }
        public DbSet<InboxMessage<ProductPriceChangedEventMessage>> ProductPriceChangedInboxMessages { get; private set; }
        public DbSet<OutboxMessage<PointsCollectedEventMessage>> PointsCollectedOutboxMessages { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
