namespace TexasTaco.Orders.Application.Customers.Exceptions
{
    internal class CustomerWithAccoundIdAlreadyExistsException(Guid accountId)
        : Exception($"Customer with {accountId} account " +
            $"Id already exists and cannot be created.");
}
