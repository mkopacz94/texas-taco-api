namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccountIdNotFoundException(Guid accountId)
        : Exception($"Customer with account id {accountId} does not exist.");
}
