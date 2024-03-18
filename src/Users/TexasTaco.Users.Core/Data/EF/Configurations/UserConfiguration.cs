using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Id)
                .HasConversion(id => id.Value, value => new UserId(value));

            builder.Property(a => a.Email)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);

            builder.HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .IsRequired();
        }
    }
}
