using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    public class ProductAmountExceededException(
        BasketItem item,
        int maximumQuantity) : BasketItemException(
            $"Exceeded maximum amount ({maximumQuantity}) of \"{item.Name}\" " +
                $"product (Id: {item.Id.Value}) in the cart.",
            ExceptionCategory.ValidationError);
}
