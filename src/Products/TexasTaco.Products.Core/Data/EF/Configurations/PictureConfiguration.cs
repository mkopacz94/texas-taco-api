using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Data.EF.Configurations
{
    internal class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasConversion(id => id.Value, value => new PictureId(value));

            builder
                .Property(p => p.Url)
                .HasMaxLength(255);

            builder
                .HasOne(p => p.Product)
                .WithOne(p => p.Picture)
                .HasForeignKey<Product>(p => p.PictureId);

            builder
                .HasOne(p => p.Prize)
                .WithOne(p => p.Picture)
                .HasForeignKey<Prize>(p => p.PictureId);
        }
    }
}
