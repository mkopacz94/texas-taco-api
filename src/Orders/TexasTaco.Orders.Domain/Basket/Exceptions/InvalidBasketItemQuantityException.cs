using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    public class InvalidBasketItemQuantityException(int quantity)
        : BasketItemException(
            $"Quantity {quantity} is an invalid value.",
            ExceptionCategory.ValidationError);
}
