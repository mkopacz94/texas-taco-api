using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class Address(string addressLine, string postalCode, string city, string country)
    {
        public AddressId Id { get; } = new AddressId(Guid.NewGuid());
        public string AddressLine { get; private set; } = addressLine;
        public string PostalCode { get; private set; } = postalCode;
        public string City { get; private set; } = city;
        public string Country { get; private set; } = country;
        public UserId? UserId { get; private set; }
        public User? User { get; private set; }

        public Address() : this("", "", "", "")
        {
        }

        public void UpdateAddress(Address address)
        {
            AddressLine = address.AddressLine;
            PostalCode = address.PostalCode;
            City = address.City;
            Country = address.Country;
        }

        public bool Contains(string value)
        {
            return AddressLine.Contains(value, StringComparison.CurrentCultureIgnoreCase)
                || PostalCode.Contains(value, StringComparison.CurrentCultureIgnoreCase)
                || City.Contains(value, StringComparison.CurrentCultureIgnoreCase)
                || Country.Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
