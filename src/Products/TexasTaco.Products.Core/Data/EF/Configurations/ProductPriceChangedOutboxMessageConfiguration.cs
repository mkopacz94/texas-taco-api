using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Products.Core.Data.EF.Configurations
{
    internal class ProductPriceChangedOutboxMessageConfiguration
        : IEntityTypeConfiguration<OutboxMessage<ProductPriceChangedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage<ProductPriceChangedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<ProductPriceChangedEventMessage>(m)!);
        }
    }
}
