using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class UserUpdatedInboxMessageConfiguration
        : IEntityTypeConfiguration<InboxMessage<UserUpdatedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<InboxMessage<UserUpdatedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<UserUpdatedEventMessage>(m)!);
        }
    }
}
