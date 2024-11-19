using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.AccountCreatedInboxMessages;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : DbContext(options)
    {
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AccountCreatedInboxMessage> AccountCreatedInboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
