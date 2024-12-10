using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccoundIdAlreadyExistsException(AccountId accountId)
        : OrdersServiceException($"Customer with {accountId.Value} account " +
            $"Id already exists and cannot be created.",
            ExceptionCategory.BadRequest);
}
