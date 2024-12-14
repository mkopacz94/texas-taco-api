using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Orders.Persistence.PointsCollectedOutboxMessages;
using TexasTaco.Shared.EventBus.Orders;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class PointsCollectedOutboxMessageConfiguration
        : IEntityTypeConfiguration<PointsCollectedOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<PointsCollectedOutboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    value => new PointsCollectedOutboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<PointsCollectedEventMessage>(m)!);
        }
    }
}
