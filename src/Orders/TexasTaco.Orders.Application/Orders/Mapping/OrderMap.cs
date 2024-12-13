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
                .Select(ol => OrderLineMap.Map(ol))
                .ToList();

            return new(
                order.Id.Value,
                order.CustomerId.Value,
                linesDtos,
                order.PaymentType,
                order.PickupLocation,
                order.TotalPrice,
                order.Status);
        }
    }
}
