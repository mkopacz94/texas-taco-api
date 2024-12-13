using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Domain.Orders
{
    public class Order
    {
        private readonly List<OrderLine> _lines = [];

        public OrderId Id { get; } = new OrderId(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; }
        public IReadOnlyCollection<OrderLine> Lines => _lines;
        public PaymentType PaymentType { get; private set; }
        public PickupLocation PickupLocation { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public string CollectOrderId => Id.Value.ToString()[..5];
        public decimal TotalPrice => Lines.Sum(p => p.UnitPrice * p.Quantity);

        protected Order(
            CustomerId customerId,
            PaymentType paymentType,
            PickupLocation pickupLocation)
        {
            CustomerId = customerId;
            PaymentType = paymentType;
            PickupLocation = pickupLocation;
        }

        private Order(
            IReadOnlyCollection<OrderLine> lines,
            CustomerId customerId,
            PaymentType paymentType,
            PickupLocation pickupLocation)
        {
            _lines = [.. lines];
            CustomerId = customerId;
            PaymentType = paymentType;
            PickupLocation = pickupLocation;
        }

        public static Order CreateFromCheckout(CheckoutCart checkoutCart)
        {
            var orderLines = checkoutCart
                .Products
                .Select((p, i) => new OrderLine(i + 1, p.Name, p.Price, p.Quantity))
                .ToList();

            return new(
                orderLines,
                checkoutCart.CustomerId,
                checkoutCart.PaymentType,
                checkoutCart.PickupLocation);
        }
    }
}
