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
        public CartId CartId { get; private set; } = null!;
        public Cart Cart { get; private set; } = null!;
        public IReadOnlyCollection<CartProduct> Products => _products;

        private CheckoutCart(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        internal CheckoutCart(Cart cart) : this(cart.CustomerId)
        {
            _products = [.. cart.Products];
            CartId = cart.Id;
        }

        public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
            => DeliveryAddress = deliveryAddress;

        public void UpdateCheckoutCart(Cart cart)
        {
            _products.Clear();
            _products.AddRange(cart.Products);
        }
    }
}
