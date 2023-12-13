using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Models
{
    public class VerificationToken(AccountId accountId, DateTime expirationDate)
    {
        public VerificationTokenId Id { get; } = new VerificationTokenId(Guid.NewGuid());
        public AccountId AccountId { get; private set; } = accountId;
        public Guid Token { get; private set; } = Guid.NewGuid();
        public DateTime ExpirationDate { get; private set; } = expirationDate;
    }
}
