using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class UserUpdatedOutboxMessageConfiguration
        : IEntityTypeConfiguration<OutboxMessage<UserUpdatedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage<UserUpdatedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<UserUpdatedEventMessage>(m)!);
        }
    }
}
