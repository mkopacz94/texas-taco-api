using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Orders.Domain.Customer;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Id)
                .HasConversion(id => id.Value, value => new CustomerId(value));

            builder.Property(a => a.Email)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasMaxLength(100);

            builder.HasOne(u => u.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<Address>(a => a.CustomerId)
                .IsRequired();

            builder
                .Navigation(u => u.Address)
                .AutoInclude();
        }
    }
}
