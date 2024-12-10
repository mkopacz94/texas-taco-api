using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public abstract class CartProductException(
        string message,
        ExceptionCategory category)
        : OrdersServiceException(message, category);
}
