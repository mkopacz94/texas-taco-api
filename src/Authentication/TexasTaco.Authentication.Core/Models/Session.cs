namespace TexasTaco.Authentication.Core.Models
{
    public class Session(DateTime expirationDate)
    {
        public DateTime ExpirationDate { get; private set; } = expirationDate;
        public bool Revoked { get; set; }

        public void ExtendSession(TimeSpan timeToExtend)
        {
            ExpirationDate = DateTime.UtcNow.Add(timeToExtend);
        }

        public void Revoke() => Revoked = true;

        public bool IsValid()
        {
            return !Revoked && DateTime.UtcNow < ExpirationDate;
        }
    }
}
