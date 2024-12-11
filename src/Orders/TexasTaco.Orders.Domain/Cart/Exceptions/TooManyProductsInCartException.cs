using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public class TooManyProductsInCartException(CartId cartId, int maximumAmountOfProducts)
        : OrdersServiceException(
            $"Exceeded maximum amount of products " +
                $"({maximumAmountOfProducts}) in the cart {cartId.Value}.",
            ExceptionCategory.ValidationError);
}
