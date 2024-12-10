using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Baskets.Exceptions
{
    internal class BasketNotFoundException(CustomerId customerId)
        : OrdersServiceException(
            $"Basket for customer with id {customerId.Value} does not exist.",
            ExceptionCategory.NotFound);
}
