using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class PointsCollectedOutboxMessageConfiguration
        : IEntityTypeConfiguration<OutboxMessage<PointsCollectedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage<PointsCollectedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<PointsCollectedEventMessage>(m)!);
        }
    }
}
