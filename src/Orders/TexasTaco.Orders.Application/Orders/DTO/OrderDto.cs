using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Orders.DTO
{
    public sealed record OrderDto(
        Guid Id,
        Guid CustomerId,
        string CollectOrderId,
        List<OrderLineDto> Lines,
        PaymentType PaymentType,
        PickupLocation PickupLocation,
        decimal TotalPrice,
        decimal PointsCollected,
        OrderStatus Status,
        DateTime OrderDate);
}
