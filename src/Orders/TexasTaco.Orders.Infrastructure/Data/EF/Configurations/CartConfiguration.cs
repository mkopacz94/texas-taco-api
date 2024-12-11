using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.CustomerId);

            builder
                .Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CartId(value));

            builder
                .Property(c => c.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder
                .Navigation(c => c.Products)
                .AutoInclude();

            builder
                .HasOne(c => c.CheckoutCart)
                .WithOne(c => c.Cart)
                .HasForeignKey<CheckoutCart>(c => c.CartId)
                .IsRequired();

            builder
                .Navigation(c => c.CheckoutCart)
                .AutoInclude();
        }
    }
}
