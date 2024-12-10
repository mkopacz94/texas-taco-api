using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Carts.Exceptions
{
    internal class CartNotFoundException(CustomerId customerId)
        : OrdersServiceException(
            $"Cart for customer with id {customerId.Value} does not exist.",
            ExceptionCategory.NotFound);
}
