using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class UserCreatedOutbox(EmailAddress userEmail)
    {
        public UserCreatedOutboxId Id { get; } = new UserCreatedOutboxId(Guid.NewGuid());
        public EmailAddress UserEmail { get; private set; } = userEmail;
        public OutboxMessageStatus MessageStatus { get; private set; } = OutboxMessageStatus.ToBePublished;

        public void MarkAsPublished() => MessageStatus = OutboxMessageStatus.Published;
    }
}
