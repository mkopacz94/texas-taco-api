using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.CheckoutCart
{
    public sealed record CheckoutCartCommand(CartId CartId)
        : IRequest<CheckoutCartDto>;
}
