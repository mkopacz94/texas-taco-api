using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public class InvalidCartProductQuantityException(int quantity)
        : CartProductException(
            $"Quantity {quantity} is an invalid value.",
            ExceptionCategory.ValidationError);
}
