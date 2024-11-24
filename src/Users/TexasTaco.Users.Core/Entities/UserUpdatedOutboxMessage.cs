using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Outbox;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class UserUpdatedOutboxMessage(UserUpdatedEventMessage messageBody)
    {
        public UserUpdatedOutboxMessageId Id { get; }
            = new UserUpdatedOutboxMessageId(Guid.NewGuid());
        public UserUpdatedEventMessage MessageBody { get; } = messageBody;
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
