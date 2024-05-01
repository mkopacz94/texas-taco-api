using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Data.EF.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Name);

            builder
                .Property(p => p.Id)
                .HasConversion(id => id.Value, value => new ProductId(value));

            builder
                .Navigation(p => p.Picture)
                .AutoInclude();
        }
    }
}
