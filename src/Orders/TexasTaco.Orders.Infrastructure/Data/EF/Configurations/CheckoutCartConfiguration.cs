using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CheckoutCartConfiguration
        : IEntityTypeConfiguration<CheckoutCart>
    {
        public void Configure(EntityTypeBuilder<CheckoutCart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.CustomerId);

            builder.Ignore(c => c.TotalPrice);

            builder
                .Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CheckoutCartId(value));

            builder
                .Property(c => c.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder
                .Navigation(c => c.Products)
                .AutoInclude();

            builder
                .Navigation(c => c.Cart)
                .AutoInclude();
        }
    }
}
