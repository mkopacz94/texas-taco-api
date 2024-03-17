using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class AccountCreatedInbox(Guid accountId, EmailAddress accountEmail)
    {
        public AccountCreatedInboxId Id { get; } = new AccountCreatedInboxId(Guid.NewGuid());
        public Guid AccountId { get; private set; } = accountId;
        public EmailAddress AccountEmail { get; private set; } = accountEmail;
        public InboxMessageStatus MessageStatus { get; private set; } = InboxMessageStatus.ToBeProcessed;
        public DateTime MessageReceived { get; private set; } = DateTime.UtcNow;
        public DateTime MessageProcessed { get; private set; }
    }
}
