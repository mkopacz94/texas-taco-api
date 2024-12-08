using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Data.EF
{
    internal class ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : DbContext(options)
    {
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<ProductPriceChangedOutboxMessage> ProductPriceChangedOutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
