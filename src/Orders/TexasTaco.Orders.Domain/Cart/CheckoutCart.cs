using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Domain.Cart
{
    public sealed class CheckoutCart
    {
        private readonly List<CartProduct> _products = [];

        public CheckoutCartId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; }
        public IReadOnlyCollection<CartProduct> Products => _products;
        public PaymentType? PaymentType { get; private set; }
        public PickupLocation? PickupLocation { get; private set; }
        public CartId CartId { get; private set; } = null!;
        public Cart Cart { get; private set; } = null!;

        public decimal TotalPrice => Products.Sum(p => p.Price * p.Quantity);

        private CheckoutCart(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        internal CheckoutCart(Cart cart) : this(cart.CustomerId)
        {
            _products = [.. cart.Products];
            CartId = cart.Id;
        }

        public void SetPaymentType(PaymentType paymentType)
            => PaymentType = paymentType;

        public void SetPickupLocation(PickupLocation pickupLocation)
            => PickupLocation = pickupLocation;

        public void UpdateCheckoutCart(Cart cart)
        {
            _products.Clear();
            _products.AddRange(cart.Products);
        }
    }
}
