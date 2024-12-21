using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class AccountDeletedOutboxMessageConfiguration
        : IEntityTypeConfiguration<AccountDeletedOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<AccountDeletedOutboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new AccountDeletedOutboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<AccountDeletedEventMessage>(m)!);
        }
    }
}
