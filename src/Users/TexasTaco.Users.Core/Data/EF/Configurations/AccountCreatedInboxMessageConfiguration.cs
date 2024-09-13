using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Data.EF.Configurations
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
