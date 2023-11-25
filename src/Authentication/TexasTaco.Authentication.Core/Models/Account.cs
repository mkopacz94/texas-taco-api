using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Models
{
    internal class Account
    {
        public AccountId Id { get; set; } = new AccountId(Guid.NewGuid());
        public EmailAddress? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
