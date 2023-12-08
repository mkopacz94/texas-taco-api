using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Models
{
    public class VerificationToken(AccountId accountId)
    {
        public VerificationTokenId Id { get; } = new VerificationTokenId(Guid.NewGuid());
        public AccountId AccountId { get; private set; } = accountId;
        public Guid Token { get; private set; } = Guid.NewGuid();
    }
}
