using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(ol => ol.Id);

            builder
                .Property(ol => ol.Id)
                .HasConversion(id => id.Value, value => new OrderLineId(value));

            builder
                .HasOne(ol => ol.Order)
                .WithMany(o => o.Lines)
                .HasForeignKey(ol => ol.OrderId)
                .IsRequired();
        }
    }
}
