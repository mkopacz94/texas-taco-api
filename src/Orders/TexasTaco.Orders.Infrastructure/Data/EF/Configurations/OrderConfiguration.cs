using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.CustomerId);

            builder.Ignore(o => o.TotalPrice);

            builder
                .Property(o => o.Id)
                .HasConversion(id => id.Value, value => new OrderId(value));

            builder
                .Property(o => o.CustomerId)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder
                .Navigation(o => o.Lines)
                .AutoInclude();
        }
    }
}
