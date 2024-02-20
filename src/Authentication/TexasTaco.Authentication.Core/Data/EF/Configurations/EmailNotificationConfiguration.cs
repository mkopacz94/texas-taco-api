using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    internal class EmailNotificationConfiguration : IEntityTypeConfiguration<EmailNotification>
    {
        public void Configure(EntityTypeBuilder<EmailNotification> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new EmailNotificationId(value));

            builder.Property(a => a.From)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);

            builder.Property(a => a.To)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);
        }
    }
}
