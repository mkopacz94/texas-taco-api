using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class User
    {
        public UserId Id { get; } = new UserId(Guid.NewGuid());
        public EmailAddress? Email { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public Address? Address { get; private set; }
        public IEnumerable<Guid>? OrdersIds { get; private set; }
    }
}
