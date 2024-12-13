using MediatR;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders.UpdateOrderStatus
{
    public sealed record UpdateOrderStatusCommand(
        OrderId Id,
        OrderStatus Status)
        : IRequest;
}
