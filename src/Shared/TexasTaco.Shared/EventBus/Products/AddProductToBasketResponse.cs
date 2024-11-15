namespace TexasTaco.Shared.EventBus.Products
{
    public record AddProductToBasketResponse(
        bool IsSuccess,
        string? ErrorMessage = null);
}
