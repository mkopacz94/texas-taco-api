using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Products
{
    public sealed record ProductPriceChangedEventMessage(
        Guid Id,
        ProductId ProductId,
        decimal NewPrice);
}
