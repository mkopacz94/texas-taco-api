using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerNotFoundException(CustomerId customerId)
        : Exception($"Customer with id {customerId.Value} does not exist.");
}
