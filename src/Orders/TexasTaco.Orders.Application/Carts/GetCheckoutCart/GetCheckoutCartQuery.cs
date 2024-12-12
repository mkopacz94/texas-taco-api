using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.GetCheckoutCart
{
    public sealed record GetCheckoutCartQuery(CheckoutCartId Id)
        : IRequest<CheckoutCartDto>;
}
