using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Inbox;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class PointsCollectedInboxMessage(PointsCollectedEventMessage messageBody)
    {
        public PointsCollectedInboxMessageId Id { get; }
            = new PointsCollectedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set; } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set; } = messageBody.Id;
        public PointsCollectedEventMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }

        public void MarkAsProcessed()
        {
            Processed = DateTime.UtcNow;
            MessageStatus = InboxMessageStatus.Processed;
        }
    }
}
