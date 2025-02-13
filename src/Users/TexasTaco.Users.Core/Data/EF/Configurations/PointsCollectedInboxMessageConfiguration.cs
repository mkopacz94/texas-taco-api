using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class PointsCollectedInboxMessageConfiguration
        : IEntityTypeConfiguration<InboxMessage<PointsCollectedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<InboxMessage<PointsCollectedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<PointsCollectedEventMessage>(m)!);
        }
    }
}
