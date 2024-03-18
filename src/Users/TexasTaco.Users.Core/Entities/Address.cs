using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class Address
    {
        public AddressId Id { get; } = new AddressId(Guid.NewGuid());
        public string? AddressLine { get; private set; }
        public string? PostalCode { get; private set; }
        public string? Country { get; private set; }
        public UserId? UserId { get; private set; }
        public User? User { get; private set; }
    }
}
