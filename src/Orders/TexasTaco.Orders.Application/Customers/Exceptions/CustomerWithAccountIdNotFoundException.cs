using TexasTaco.Orders.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccountIdNotFoundException(Guid accountId)
        : OrdersServiceException(
            $"Customer with account id {accountId} does not exist.",
            ExceptionCategory.NotFound);
}
