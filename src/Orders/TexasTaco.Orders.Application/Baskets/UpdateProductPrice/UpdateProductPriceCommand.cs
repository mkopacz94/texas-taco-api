using MediatR;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Baskets.UpdateProductPrice
{
    public sealed record UpdateProductPriceCommand(
        ProductId ProductId,
        decimal NewPrice) : IRequest;
}
