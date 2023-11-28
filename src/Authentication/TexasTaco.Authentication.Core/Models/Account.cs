using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Models
{
    public class Account(EmailAddress email, Role role, byte[] passwordHash, byte[] passwordSalt)
    {
        public AccountId Id { get; set; } = new AccountId(Guid.NewGuid());
        public EmailAddress Email { get; set; } = email;
        public Role Role { get; set; } = role;
        public byte[] PasswordHash { get; set; } = passwordHash;
        public byte[] PasswordSalt { get; set; } = passwordSalt;
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    }
}
