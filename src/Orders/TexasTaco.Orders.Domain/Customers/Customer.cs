using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Customers
{
    public sealed class Customer(AccountId accountId, EmailAddress email)
    {
        public CustomerId Id { get; } = new CustomerId(Guid.NewGuid());
        public AccountId AccountId { get; private set; } = accountId;
        public EmailAddress Email { get; private set; } = email;
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public Address Address { get; private set; } = new Address();
        public int PointsCollected { get; private set; }

        public void UpdateCustomer(string firstName, string lastName, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Address.UpdateAddress(address);
        }

        public void AddPoints(Order order)
            => PointsCollected += order.PointsCollected;
    }
}
