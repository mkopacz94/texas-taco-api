using TexasTaco.Orders.Application.Customers.DTO;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.Mapping
{
    internal static class AddressMap
    {
        public static AddressDto Map(Address address)
        {
            return new(
                address.AddressLine,
                address.PostalCode,
                address.City,
                address.Country);
        }
    }
}
