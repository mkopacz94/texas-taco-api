using TexasTaco.Orders.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccoundIdAlreadyExistsException(Guid accountId)
        : OrdersServiceException($"Customer with {accountId} account " +
            $"Id already exists and cannot be created.",
            ExceptionCategory.BadRequest);
}
