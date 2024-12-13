using TexasTaco.Orders.Domain.Orders.Exceptions;

namespace TexasTaco.Orders.Domain.Orders
{
    public sealed class OrderLine
    {
        public OrderLineId Id { get; } = new OrderLineId(Guid.NewGuid());
        public int OrderLineNumber { get; private set; }
        public string Name { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public OrderId OrderId { get; private set; } = null!;
        public Order Order { get; private set; } = null!;

        public OrderLine(int orderLineNumber, string name, decimal unitPrice, int quantity)
        {
            if (quantity < 1)
            {
                throw new InvalidOrderLineDataException(
                    $"Quantity {quantity} is not valid quantity value.");
            }

            if (unitPrice < 0)
            {
                throw new InvalidOrderLineDataException(
                    $"Unit price {unitPrice} is not valid unit price value.");
            }

            OrderLineNumber = orderLineNumber;
            Name = name;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
