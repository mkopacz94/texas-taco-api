namespace TexasTaco.Authentication.Core.Models
{
    public class Session(DateTime expirationDate)
    {
        public DateTime ExpirationDate { get; set; } = expirationDate;
    }
}
