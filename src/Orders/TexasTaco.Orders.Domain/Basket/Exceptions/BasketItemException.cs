using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    public abstract class BasketItemException(
        string message,
        ExceptionCategory category)
        : OrdersServiceException(message, category);
}
