using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public class CartProductNotFoundException(CartProductId id)
        : OrdersServiceException(
            $"Cart product with ID {id.Value} not found in the cart.",
            ExceptionCategory.NotFound);
}
