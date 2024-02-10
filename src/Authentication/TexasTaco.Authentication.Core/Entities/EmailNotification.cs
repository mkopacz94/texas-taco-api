using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class EmailNotification(string subject, string body, EmailAddress from, EmailAddress to)
    {
        public EmailNotificationId Id { get; } = new EmailNotificationId(Guid.NewGuid());
        public string Subject { get; private set; } = subject;
        public string Body { get; private set; } = body;
        public EmailAddress From { get; private set; } = from;
        public EmailAddress To { get; private set; } = to;
        public EmailNotificationStatus Status { get; private set; } = EmailNotificationStatus.Pending;

        public void MarkAsSent() => Status = EmailNotificationStatus.Sent;
    }
}
