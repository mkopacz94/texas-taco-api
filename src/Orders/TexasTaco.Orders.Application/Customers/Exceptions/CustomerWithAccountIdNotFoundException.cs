using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccountIdNotFoundException(AccountId accountId)
        : OrdersServiceException(
            $"Customer with account id {accountId.Value} does not exist.",
            ExceptionCategory.NotFound);
}
