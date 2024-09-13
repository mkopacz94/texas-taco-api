using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class AccountCreatedInboxMessage(AccountCreatedEventMessage messageBody)
    {
        public AccountCreatedInboxMessageId Id { get; } = new AccountCreatedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set;  } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set;  } = messageBody.Id;
        public AccountCreatedEventMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }
    }
}
