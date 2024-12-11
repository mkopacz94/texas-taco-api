using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Customers
{
    public sealed class Address(string addressLine, string postalCode, string city, string country)
    {
        public AddressId Id { get; } = new AddressId(Guid.NewGuid());
        public string AddressLine { get; private set; } = addressLine;
        public string PostalCode { get; private set; } = postalCode;
        public string City { get; private set; } = city;
        public string Country { get; private set; } = country;
        public CustomerId? CustomerId { get; private set; }
        public Customer? Customer { get; private set; }

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
    }
}
