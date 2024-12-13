using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Domain.Orders
{
    public sealed class Order
    {
        private readonly List<OrderLine> _lines = [];

        public OrderId Id { get; } = new OrderId(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; }
        public IReadOnlyCollection<OrderLine> Lines => _lines;
        public PaymentType PaymentType { get; private set; }
        public PickupLocation PickupLocation { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalPrice => Lines.Sum(p => p.UnitPrice * p.Quantity);

        public Order(
            CustomerId customerId,
            PaymentType paymentType,
            PickupLocation pickupLocation,
            OrderStatus status)
        {
            CustomerId = customerId;
            PaymentType = paymentType;
            PickupLocation = pickupLocation;
            Status = status;
        }

        public Order(
            IReadOnlyCollection<OrderLine> lines,
            CustomerId customerId,
            PaymentType paymentType,
            PickupLocation pickupLocation,
            OrderStatus status)
        {
            _lines = [.. lines];
            CustomerId = customerId;
            PaymentType = paymentType;
            PickupLocation = pickupLocation;
            Status = status;
        }
    }
}
