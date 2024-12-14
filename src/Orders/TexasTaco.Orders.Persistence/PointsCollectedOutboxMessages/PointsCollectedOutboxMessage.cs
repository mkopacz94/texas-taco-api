using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Orders.Persistence.PointsCollectedOutboxMessages
{
    public sealed class PointsCollectedOutboxMessage(
        PointsCollectedEventMessage messageBody)
    {
        public PointsCollectedOutboxMessageId Id { get; }
            = new PointsCollectedOutboxMessageId(Guid.NewGuid());
        public PointsCollectedEventMessage MessageBody { get; } = messageBody;
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
