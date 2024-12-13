using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders.Mapping
{
    internal static class OrderLineMap
    {
        public static OrderLineDto Map(OrderLine orderLine)
        {
            return new OrderLineDto(
                orderLine.Id.Value,
                orderLine.OrderLineNumber,
                orderLine.Name,
                orderLine.UnitPrice,
                orderLine.Quantity);
        }
    }
}
