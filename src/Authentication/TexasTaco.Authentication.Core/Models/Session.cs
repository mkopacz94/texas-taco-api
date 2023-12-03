namespace TexasTaco.Authentication.Core.Models
{
    public class Session(DateTime expirationDate)
    {
        public DateTime ExpirationDate { get; private set; } = expirationDate;

        public void ExtendSession(TimeSpan timeToExtend)
        {
            ExpirationDate = DateTime.UtcNow.Add(timeToExtend);
        }
    }
}
