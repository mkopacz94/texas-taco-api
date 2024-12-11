using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CheckoutCartConfiguration
        : IEntityTypeConfiguration<CheckoutCart>
    {
        public void Configure(EntityTypeBuilder<CheckoutCart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.CustomerId);

            builder
                .Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CheckoutCartId(value));

            builder
                .Property(c => c.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder.HasOne(c => c.DeliveryAddress)
                .WithOne(da => da.CheckoutCart)
                .HasForeignKey<DeliveryAddress>(da => da.CheckoutCartId);

            builder
                .Navigation(u => u.DeliveryAddress)
                .AutoInclude();
        }
    }
}
