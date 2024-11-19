using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.CustomerId);

            builder
                .Property(b => b.Id)
                .HasConversion(id => id.Value, value => new BasketId(value));

            builder
                .Property(b => b.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));
        }
    }
}
