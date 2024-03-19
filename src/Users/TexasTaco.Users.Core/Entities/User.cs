using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Entities
{
    public class User(Guid accountId, EmailAddress email)
    {
        public UserId Id { get; } = new UserId(Guid.NewGuid());
        public Guid AccountId { get; private set; } = accountId;
        public EmailAddress Email { get; private set; } = email;
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public Address? Address { get; private set; }

        public void UpdateUser(string firstName, string lastName, Address address) 
        { 
            FirstName = firstName;
            LastName = lastName;

            if (Address is null)
            {
                Address = new Address(address.AddressLine, address.PostalCode, address.Country);
            }
            else
            {
                Address?.UpdateAddress(address);
            }
        }
    }
}
