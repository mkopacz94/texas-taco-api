using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class VerificationToken(AccountId accountId, DateTime expirationDate)
    {
        public VerificationTokenId Id { get; } = new VerificationTokenId(Guid.NewGuid());
        public AccountId AccountId { get; private set; } = accountId;
        public Guid Token { get; private set; } = Guid.NewGuid();
        public DateTime ExpirationDate { get; private set; } = expirationDate;

        public bool IsExpired => ExpirationDate < DateTime.UtcNow;
    }
}
