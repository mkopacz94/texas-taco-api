using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Carts.Exceptions
{
    public class CheckoutCartNotFoundException(CheckoutCartId id)
        : OrdersServiceException(
            $"Checkout cart with ID {id.Value} does not exist.",
            ExceptionCategory.NotFound);
}
