using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class PointsCollectedInboxMessageConfiguration
        : IEntityTypeConfiguration<PointsCollectedInboxMessage>
    {
        public void Configure(EntityTypeBuilder<PointsCollectedInboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ValueObjects.PointsCollectedInboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<PointsCollectedEventMessage>(m)!);
        }
    }
}
