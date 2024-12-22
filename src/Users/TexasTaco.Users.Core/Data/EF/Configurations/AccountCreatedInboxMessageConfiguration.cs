using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class AccountCreatedInboxMessageConfiguration
        : IEntityTypeConfiguration<InboxMessage<AccountCreatedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<InboxMessage<AccountCreatedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<AccountCreatedEventMessage>(m)!);
        }
    }
}
