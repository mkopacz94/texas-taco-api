using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Domain.Orders
{
    public class Order
    {
        private readonly List<OrderLine> _lines = [];

        public OrderId Id { get; } = new OrderId(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; } = null!;
        public Customer Customer { get; private set; } = null!;
        public IReadOnlyCollection<OrderLine> Lines => _lines;
        public PaymentType PaymentType { get; private set; }
        public PickupLocation PickupLocation { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public string CollectOrderId => Id.Value.ToString()[..5];
        public int PointsCollected { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

        protected Order(
            CustomerId customerId,
            PaymentType paymentType,
            PickupLocation pickupLocation,
            int pointsCollected,
            decimal totalPrice)
        {
            CustomerId = customerId;
            PaymentType = paymentType;
            PickupLocation = pickupLocation;
            PointsCollected = pointsCollected;
            TotalPrice = totalPrice;
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
            PointsCollected = CalculatePoints();
            TotalPrice = Lines.Sum(p => p.UnitPrice * p.Quantity);
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

        public void UpdateStatus(OrderStatus status) => Status = status;

        private int CalculatePoints()
            => (int)Lines.Sum(l =>
                Math.Ceiling(l.UnitPrice * l.Quantity * 10));
    }
}
