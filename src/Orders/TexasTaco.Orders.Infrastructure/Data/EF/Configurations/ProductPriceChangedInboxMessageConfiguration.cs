using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Orders.Persistence.ProductPriceChangedInbox;
using TexasTaco.Shared.EventBus.Products;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class ProductPriceChangedInboxMessageConfiguration
        : IEntityTypeConfiguration<ProductPriceChangedInboxMessage>
    {
        public void Configure(EntityTypeBuilder<ProductPriceChangedInboxMessage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ProductPriceChangedInboxMessageId(value));

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<ProductPriceChangedEventMessage>(m)!);
        }
    }
}
