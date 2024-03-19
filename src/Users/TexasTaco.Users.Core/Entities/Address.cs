using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class Address(string addressLine, string postalCode, string country)
    {
        public AddressId Id { get; } = new AddressId(Guid.NewGuid());
        public string AddressLine { get; private set; } = addressLine;
        public string PostalCode { get; private set; } = postalCode;
        public string Country { get; private set; } = country;
        public UserId? UserId { get; private set; }
        public User? User { get; private set; }

        public void UpdateAddress(Address address)
        {
            AddressLine = address.AddressLine;
            PostalCode = address.PostalCode;
            Country = address.Country;
        }
    }
}
