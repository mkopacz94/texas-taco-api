using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class AccountCreatedInboxMessage(AccountCreatedEventMessage message)
    {
        public AccountCreatedInboxMessageId Id { get; } = new AccountCreatedInboxMessageId(Guid.NewGuid());
        public DateTime Received { get; private set; }
        public DateTime Processed { get; private set; }
        public AccountCreatedEventMessage Message { get; } = message;
        public InboxMessageStatus MessageStatus { get; private set; }
    }
}
