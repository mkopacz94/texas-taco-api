using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Products.Core.Data.EF
{
    internal class ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : DbContext(options)
    {
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OutboxMessage<ProductPriceChangedEventMessage>> ProductPriceChangedOutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
