using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public class CannotCheckoutEmptyCartException()
        : OrdersServiceException(
            "Empty cart cannot be checked out.",
            ExceptionCategory.ValidationError);
}
