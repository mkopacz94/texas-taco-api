using MediatR;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts.UpdateProductPrice
{
    public sealed record UpdateProductPriceCommand(
        ProductId ProductId,
        decimal NewPrice) : IRequest;
}
