using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class AccountCreatedInboxConfiguration : IEntityTypeConfiguration<AccountCreatedInbox>
    {
        public void Configure(EntityTypeBuilder<AccountCreatedInbox> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new AccountCreatedInboxId(value));

            builder.Property(a => a.AccountEmail)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);
        }
    }
}
