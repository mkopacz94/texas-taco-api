using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders.Mapping
{
    internal static class OrderMap
    {
        public static OrderDto Map(Order order)
        {
            var linesDtos = order
                .Lines
                .Select(OrderLineMap.Map)
                .OrderBy(ol => ol.OrderLineNumber)
                .ToList();

            return new(
                order.Id.Value,
                order.CustomerId.Value,
                order.CollectOrderId,
                linesDtos,
                order.PaymentType,
                order.PickupLocation,
                order.TotalPrice,
                order.PointsCollected,
                order.Status,
                order.OrderDate);
        }
    }
}
