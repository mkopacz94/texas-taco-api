using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class AccountCreatedOutboxConfiguration : IEntityTypeConfiguration<AccountCreatedOutbox>
    {
        public void Configure(EntityTypeBuilder<AccountCreatedOutbox> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new AccountCreatedOutboxId(value));

            builder.Property(a => a.AccountId)
                .HasConversion(id => id.Value, value => new AccountId(value));

            builder.Property(a => a.UserEmail)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);
        }
    }
}
