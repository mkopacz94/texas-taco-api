using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Domain.Cart
{
    public sealed class CheckoutCart
    {
        private readonly List<CartProduct> _products = [];

        public CheckoutCartId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; }
        public DeliveryAddress? DeliveryAddress { get; private set; }
        public IReadOnlyCollection<CartProduct> Products => _products;

        private CheckoutCart(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        internal CheckoutCart(Cart cart) : this(cart.CustomerId)
        {
            _products = [.. cart.Products];
        }

        public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
            => DeliveryAddress = deliveryAddress;
    }
}
