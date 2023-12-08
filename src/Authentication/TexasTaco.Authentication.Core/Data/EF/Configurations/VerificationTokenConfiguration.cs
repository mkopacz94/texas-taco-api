using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Data.EF.Configurations
{
    public class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
    {
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder.HasKey(vt => vt.Id);
            builder.HasIndex(vt => vt.Id);

            builder.Property(vt => vt.Id)
                .HasConversion(id => id.Value, value => new VerificationTokenId(value));

            builder.Property(vt => vt.AccountId)
                .HasConversion(id => id.Value, value => new AccountId(value));
        }
    }
}
