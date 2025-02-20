using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Entities
{
    public class Account(EmailAddress email, Role role, byte[] passwordHash, byte[] passwordSalt)
    {
        public AccountId Id { get; } = new AccountId(Guid.NewGuid());
        public EmailAddress Email { get; private set; } = email;
        public Role Role { get; private set; } = role;
        public byte[] PasswordHash { get; private set; } = passwordHash;
        public byte[] PasswordSalt { get; private set; } = passwordSalt;
        public DateTime RegisterDate { get; private set; } = DateTime.UtcNow;
        public bool Verified { get; private set; }

        public void MarkAsVerified() => Verified = true;
        public bool IsInRole(string role)
            => string.Equals(role, Role.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
