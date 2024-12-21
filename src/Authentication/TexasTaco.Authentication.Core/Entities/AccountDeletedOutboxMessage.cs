using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Authentication.Core.Entities
{
    public class AccountDeletedOutboxMessage(AccountDeletedEventMessage messageBody)
    {
        public AccountDeletedOutboxMessageId Id { get; }
            = new AccountDeletedOutboxMessageId(Guid.NewGuid());
        public AccountDeletedEventMessage MessageBody { get; } = messageBody;
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
