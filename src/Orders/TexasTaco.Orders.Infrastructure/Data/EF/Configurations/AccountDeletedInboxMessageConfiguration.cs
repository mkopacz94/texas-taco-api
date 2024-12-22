using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class AccountDeletedInboxMessageConfiguration
        : IEntityTypeConfiguration<InboxMessage<AccountDeletedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<InboxMessage<AccountDeletedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<AccountDeletedEventMessage>(m)!);
        }
    }
}
