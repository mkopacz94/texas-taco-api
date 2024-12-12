using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.UpdateProductQuantity
{
    public sealed record UpdateProductQuantityCommand(
        CartId CartId,
        CartProductId ProductId,
        int Quantity)
        : IRequest<CartProductDto>;
}
