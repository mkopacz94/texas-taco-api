using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Domain.Basket;

namespace TexasTaco.Orders.Infrastructure.Data.EF
{
    internal class OrdersDbContextt(DbContextOptions<OrdersDbContextt> options)
        : DbContext(options)
    {
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
