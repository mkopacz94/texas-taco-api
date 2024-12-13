using MediatR;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.PlaceOrder
{
    public sealed record PlaceOrderCommand(CheckoutCartId CheckoutCartId)
        : IRequest<OrderDto>;
}
