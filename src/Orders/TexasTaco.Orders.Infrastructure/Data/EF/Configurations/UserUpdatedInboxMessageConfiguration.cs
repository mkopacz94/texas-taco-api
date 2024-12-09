using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Orders.Persistence.UserUpdatedInboxMessages;
using TexasTaco.Shared.EventBus.Users;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class UserUpdatedInboxMessageConfiguration
        : IEntityTypeConfiguration<UserUpdatedInboxMessage>
    {
        public void Configure(EntityTypeBuilder<UserUpdatedInboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new UserUpdatedInboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<UserUpdatedEventMessage>(m)!);
        }
    }
}
