using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Customer
{
    public class Customer(Guid accountId, EmailAddress email)
    {
        public CustomerId Id { get; } = new CustomerId(Guid.NewGuid());
        public Guid AccountId { get; private set; } = accountId;
        public EmailAddress Email { get; private set; } = email;
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public Address Address { get; private set; } = new Address();
        public int PointsCollected { get; private set; }
    }
}
