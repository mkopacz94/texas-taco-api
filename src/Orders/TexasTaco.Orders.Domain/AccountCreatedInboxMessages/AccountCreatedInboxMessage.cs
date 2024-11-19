using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Domain.AccountCreatedInboxMessages
{
    public class AccountCreatedInboxMessage(AccountCreatedEventMessage messageBody)
    {
        public AccountCreatedInboxMessageId Id { get; } = new AccountCreatedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set; } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set; } = messageBody.Id;
        public AccountCreatedEventMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }

        public void MarkAsProcessed()
        {
            Processed = DateTime.UtcNow;
            MessageStatus = InboxMessageStatus.Processed;
        }
    }
}
