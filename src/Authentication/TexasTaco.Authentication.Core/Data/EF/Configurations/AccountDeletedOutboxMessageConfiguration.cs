using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class AccountDeletedOutboxMessageConfiguration
        : IEntityTypeConfiguration<OutboxMessage<AccountDeletedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage<AccountDeletedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<AccountDeletedEventMessage>(m)!);
        }
    }
}
