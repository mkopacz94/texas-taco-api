using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Orders.Persistence.AccountCreatedInboxMessages;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class AccountCreatedInboxMessageConfiguration
        : IEntityTypeConfiguration<AccountCreatedInboxMessage>
    {
        public void Configure(EntityTypeBuilder<AccountCreatedInboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new AccountCreatedInboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<AccountCreatedEventMessage>(m)!);
        }
    }
}
