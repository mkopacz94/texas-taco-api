using MediatR;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Orders;

namespace TexasTaco.Orders.Application.Orders.GetOrder
{
    public sealed record GetOrderQuery(OrderId Id)
        : IRequest<OrderDto>;
}
