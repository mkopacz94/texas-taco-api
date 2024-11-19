using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Data.EF.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.AddressLine);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new AddressId(value));
        }
    }
}
