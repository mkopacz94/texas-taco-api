using TexasTaco.Orders.Application.Customers.DTO;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.Mapping
{
    internal static class CustomerMap
    {
        public static CustomerDto Map(Customer customer)
        {
            return new(
                customer.Id.Value,
                customer.Email.Value,
                customer.FirstName,
                customer.LastName,
                AddressMap.Map(customer.Address),
                customer.PointsCollected);
        }
    }
}
