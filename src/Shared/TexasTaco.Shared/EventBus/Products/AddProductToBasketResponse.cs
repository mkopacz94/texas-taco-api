namespace TexasTaco.Shared.EventBus.Products
{
    public sealed record AddProductToBasketResponse(
        bool IsSuccess,
        string? ErrorMessage = null);
}
