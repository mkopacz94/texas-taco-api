using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class UserCreatedOutboxConfiguration : IEntityTypeConfiguration<UserCreatedOutbox>
    {
        public void Configure(EntityTypeBuilder<UserCreatedOutbox> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(id => id.Value, value => new UserCreatedOutboxId(value));

            builder.Property(u => u.UserEmail)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);
        }
    }
}
