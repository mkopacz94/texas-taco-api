using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .HasConversion(id => id.Value, value => new CartProductId(value));

            builder
                .Property(b => b.ProductId)
                .HasConversion(id => id.Value, value => new ProductId(value));

            builder
                .HasOne(b => b.Cart)
                .WithMany(b => b.Products)
                .HasForeignKey(b => b.CartId)
                .IsRequired();
        }
    }
}
