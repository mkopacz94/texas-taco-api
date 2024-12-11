using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Domain.Shared
{
    public class DeliveryAddress(
        string receiverFullName,
        string addressLine,
        string postalCode,
        string city)
    {
        public DeliveryAddressId Id { get; } = new DeliveryAddressId(Guid.NewGuid());
        public string ReceiverFullName { get; private set; } = receiverFullName;
        public string AddressLine { get; private set; } = addressLine;
        public string PostalCode { get; private set; } = postalCode;
        public string City { get; private set; } = city;
        public CheckoutCartId? CheckoutCartId { get; private set; }
        public CheckoutCart? CheckoutCart { get; private set; }
    }
}
