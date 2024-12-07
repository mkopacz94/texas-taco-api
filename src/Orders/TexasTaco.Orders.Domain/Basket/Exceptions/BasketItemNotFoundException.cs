using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    internal class BasketItemNotFoundException(ProductId productId)
        : OrdersServiceException(
            $"Product with {productId.Value} id not found in the basket.",
            ExceptionCategory.NotFound);
}
