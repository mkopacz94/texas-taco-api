using TexasTaco.Orders.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    public class InvalidBasketItemQuantityException(int quantity)
        : OrdersServiceException(
            $"Quantity {quantity} is invalid value.",
            ExceptionCategory.ValidationError);
}
