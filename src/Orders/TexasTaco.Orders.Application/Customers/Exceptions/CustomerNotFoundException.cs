using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerNotFoundException : OrdersServiceException
    {
        public CustomerNotFoundException(CustomerId customerId)
            : base($"Customer with id {customerId.Value} does not exist.",
                  ExceptionCategory.NotFound)
        { }

        public CustomerNotFoundException(AccountId accountId)
            : base($"Customer for account id {accountId.Value} does not exist.",
                  ExceptionCategory.NotFound)
        { }
    }
}
