using MediatR;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts.AddProductToCart
{
    public sealed record AddProductToCartCommand(
        AccountId AccountId,
        CartProduct Item) : IRequest<Cart>;
}
