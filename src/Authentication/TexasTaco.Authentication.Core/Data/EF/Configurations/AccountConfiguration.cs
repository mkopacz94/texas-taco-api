using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.Email);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new AccountId(value));

            builder.Property(a => a.Email)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);
        }
    }
}
