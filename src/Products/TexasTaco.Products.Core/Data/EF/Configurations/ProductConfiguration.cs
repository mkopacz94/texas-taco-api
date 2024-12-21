using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Data.EF.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Name);
            builder.Ignore(p => p.PriceChanged);

            builder
                .Property(p => p.Id)
                .HasConversion(id => id.Value, value => new ProductId(value));

            builder
                .Navigation(p => p.Picture)
                .AutoInclude();

            builder
                .Navigation(p => p.Category)
                .AutoInclude();

            builder
                .HasMany(p => p.Prizes)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            builder
                .Metadata
                .FindNavigation("Prizes")!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
