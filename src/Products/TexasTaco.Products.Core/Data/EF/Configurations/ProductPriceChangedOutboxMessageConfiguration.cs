using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.EventBus.Products;

namespace TexasTaco.Products.Core.Data.EF.Configurations
{
    internal class ProductPriceChangedOutboxMessageConfiguration
        : IEntityTypeConfiguration<ProductPriceChangedOutboxMessage>
    {
        public void Configure(EntityTypeBuilder<ProductPriceChangedOutboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new ProductPriceChangedOutboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<ProductPriceChangedEventMessage>(m)!);
        }
    }
}
