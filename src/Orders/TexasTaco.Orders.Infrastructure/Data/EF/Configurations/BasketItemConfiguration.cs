using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .HasConversion(id => id.Value, value => new BasketItemId(value));

            builder
                .Property(b => b.ProductId)
                .HasConversion(id => id.Value, value => new ProductId(value));

            builder
                .HasOne(b => b.Basket)
                .WithMany(b => b.Items)
                .HasForeignKey(b => b.BasketId)
                .IsRequired();
        }
    }
}
