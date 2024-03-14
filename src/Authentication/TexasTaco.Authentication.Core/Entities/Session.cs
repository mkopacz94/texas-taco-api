namespace TexasTaco.Authentication.Core.Entities
{
    public class Session(DateTime lastExtensionDate, DateTime expirationDate)
    {
        public DateTime LastExtensionDate { get; private set; } = lastExtensionDate;
        public DateTime ExpirationDate { get; private set; } = expirationDate;
        public bool Revoked { get; set; }

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
    }
}
