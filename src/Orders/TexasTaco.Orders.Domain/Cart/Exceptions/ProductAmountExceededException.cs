using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Cart.Exceptions
{
    public class ProductAmountExceededException(
        CartProduct item,
        int maximumQuantity) : CartProductException(
            $"Exceeded maximum amount ({maximumQuantity}) of \"{item.Name}\" " +
                $"product (Id: {item.Id.Value}) in the cart.",
            ExceptionCategory.ValidationError);
}
