using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customer;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : DbContext(options)
    {
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
