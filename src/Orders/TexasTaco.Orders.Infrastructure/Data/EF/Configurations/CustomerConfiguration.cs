using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasIndex(c => c.Email)
                .IsUnique();

            builder
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            builder.Property(c => c.AccountId)
               .HasConversion(id => id.Value, value => new AccountId(value));

            builder.Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder.Property(a => a.Email)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);

            builder.HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<Address>(a => a.CustomerId)
                .IsRequired();

            builder
                .Navigation(c => c.Address)
                .AutoInclude();

            builder
                .Navigation(c => c.Orders)
                .AutoInclude();
        }
    }
}
