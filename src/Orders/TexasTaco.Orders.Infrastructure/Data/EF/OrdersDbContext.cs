using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.AccountCreatedInboxMessages;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.UserUpdatedInboxMessages;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : DbContext(options)
    {
        public DbSet<BasketItem> BasketItems { get; private set; }
        public DbSet<Basket> Baskets { get; private set; }
        public DbSet<Customer> Customers { get; private set; }
        public DbSet<AccountCreatedInboxMessage> AccountCreatedInboxMessages { get; private set; }
        public DbSet<UserUpdatedInboxMessage> UserUpdatedInboxMessages { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
