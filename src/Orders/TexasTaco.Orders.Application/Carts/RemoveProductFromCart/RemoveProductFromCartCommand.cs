using MediatR;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.RemoveProductFromCart
{
    public sealed record RemoveProductFromCartCommand(
        CartId CartId,
        CartProductId CartProductId)
        : IRequest;
}
