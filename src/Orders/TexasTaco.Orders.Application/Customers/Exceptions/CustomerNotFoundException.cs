using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerNotFoundException(CustomerId customerId)
        : OrdersServiceException(
            $"Customer with id {customerId.Value} does not exist.",
            ExceptionCategory.NotFound);
}
