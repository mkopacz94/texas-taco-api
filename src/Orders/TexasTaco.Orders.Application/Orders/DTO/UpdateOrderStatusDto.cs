using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders.DTO
{
    public sealed record UpdateOrderStatusDto(OrderStatus Status);
}
