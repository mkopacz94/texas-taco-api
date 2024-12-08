using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Products.Core.Entities
{
    public class ProductPriceChangedOutboxMessage(ProductPriceChangedEventMessage messageBody)
    {
        public ProductPriceChangedOutboxMessageId Id { get; }
            = new ProductPriceChangedOutboxMessageId(Guid.NewGuid());
        public ProductPriceChangedEventMessage MessageBody { get; } = messageBody;
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public DateTime Published { get; private set; }
        public OutboxMessageStatus MessageStatus { get; private set; }
            = OutboxMessageStatus.ToBePublished;

        public void MarkAsPublished()
        {
            Published = DateTime.UtcNow;
            MessageStatus = OutboxMessageStatus.Published;
        }
    }
}
