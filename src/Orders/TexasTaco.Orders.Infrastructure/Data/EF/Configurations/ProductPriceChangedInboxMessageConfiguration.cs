using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Infrastructure.Data.EF.Configurations
{
    internal class ProductPriceChangedInboxMessageConfiguration
        : IEntityTypeConfiguration<InboxMessage<ProductPriceChangedEventMessage>>
    {
        public void Configure(EntityTypeBuilder<InboxMessage<ProductPriceChangedEventMessage>> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageBody)
                .HasConversion(
                    m => JsonConvert.SerializeObject(m),
                    m => JsonConvert.DeserializeObject<ProductPriceChangedEventMessage>(m)!);
        }
    }
}
