using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Products
{
    public record AddProductToBasketRequest(
        ProductId ProductId,
        string Name,
        decimal Price,
        string? PictureUrl,
        int Quantity);
}
