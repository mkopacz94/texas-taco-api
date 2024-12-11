using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class DeliveryAddressConfiguration
        : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.HasKey(da => da.Id);
            builder.HasIndex(da => da.ReceiverFullName);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new DeliveryAddressId(value));
        }
    }
}
