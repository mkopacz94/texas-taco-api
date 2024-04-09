using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class Session(
        SessionId id, 
        DateTime lastExtensionDate,
        DateTime expirationDate,
        bool revoked)
    {
        public SessionId Id { get; private set; } = id;
        public DateTime LastExtensionDate { get; private set; } = lastExtensionDate;
        public DateTime ExpirationDate { get; private set; } = expirationDate;
        public bool Revoked { get; private set; } = revoked;

        public bool IsBeforeHalfOfExpirationTime()
        {
            var difference = ExpirationDate - LastExtensionDate;
            var halfOfDifference = difference.Divide(2);

            return DateTime.UtcNow < LastExtensionDate.Add(halfOfDifference);
        }

        public void ExtendSession(TimeSpan timeToExtend)
        {
            LastExtensionDate = DateTime.UtcNow;
            ExpirationDate = DateTime.UtcNow.Add(timeToExtend);
        }

        public void Revoke() => Revoked = true;

        public bool IsValid()
        {
            return !Revoked && DateTime.UtcNow < ExpirationDate;
        }

        public void Update(Session session)
        {
            LastExtensionDate = session.LastExtensionDate;
            ExpirationDate = session.ExpirationDate;
            Revoked = session.Revoked;
        }
    }
}
