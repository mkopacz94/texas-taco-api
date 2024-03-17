using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class AccountCreatedOutbox(AccountId accountId, EmailAddress userEmail)
    {
        public AccountCreatedOutboxId Id { get; } = new AccountCreatedOutboxId(Guid.NewGuid());
        public AccountId AccountId { get; private set; } = accountId;
        public EmailAddress UserEmail { get; private set; } = userEmail;
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public DateTime Published { get; private set; }
        public OutboxMessageStatus MessageStatus { get; private set; } = OutboxMessageStatus.ToBePublished;

        public void MarkAsPublished()
        {
            Published = DateTime.UtcNow;
            MessageStatus = OutboxMessageStatus.Published;
        }
    }
}
