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
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.CustomerId);

            builder
                .Property(b => b.Id)
                .HasConversion(id => id.Value, value => new CartId(value));

            builder
                .Property(b => b.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder
                .Navigation(b => b.Products)
                .AutoInclude();
        }
    }
}
