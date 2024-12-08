using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Persistence.ProductPriceChangedInbox
{
    public class ProductPriceChangedInboxMessage(
        ProductPriceChangedEventMessage messageBody)
    {
        public ProductPriceChangedInboxMessageId Id { get; }
            = new ProductPriceChangedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set; } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set; } = messageBody.Id;
        public ProductPriceChangedEventMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }

        public void MarkAsProcessed()
        {
            Processed = DateTime.UtcNow;
            MessageStatus = InboxMessageStatus.Processed;
        }
    }
}
