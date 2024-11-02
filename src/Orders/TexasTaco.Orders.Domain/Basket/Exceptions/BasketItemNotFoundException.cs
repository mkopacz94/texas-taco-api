using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    internal class BasketItemNotFoundException(ProductId productId)
        : Exception($"Product with {productId.Value} id not found in the basket.");
}
