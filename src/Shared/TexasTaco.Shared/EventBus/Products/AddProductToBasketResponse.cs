namespace TexasTaco.Shared.EventBus.Products
{
    public sealed record AddProductToBasketResponse(
        bool IsSuccess,
        string? ProductLocation = null,
        string? ErrorMessage = null);
}
