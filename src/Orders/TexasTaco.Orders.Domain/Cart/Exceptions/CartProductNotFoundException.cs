using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    internal class CartProductNotFoundException(ProductId productId)
        : OrdersServiceException(
            $"Product with {productId.Value} id not found in the cart.",
            ExceptionCategory.NotFound);
}
