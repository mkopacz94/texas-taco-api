using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Persistence.UserUpdatedInboxMessages
{
    public class UserUpdatedInboxMessage(UserUpdatedEventMessage messageBody)
    {
        public UserUpdatedInboxMessageId Id { get; }
            = new UserUpdatedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set; } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set; } = messageBody.Id;
        public UserUpdatedEventMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }

        public void MarkAsProcessed()
        {
            Processed = DateTime.UtcNow;
            MessageStatus = InboxMessageStatus.Processed;
        }
    }
}
