using TexasTaco.Orders.Application.Carts.DTO;

namespace TexasTaco.Orders.Application.Carts.Mapping
{
    internal static class CheckoutCartMap
    {
        public static CheckoutCartDto Map(Domain.Cart.CheckoutCart checkoutCart)
        {
            var productsDto = checkoutCart
                .Products
                .Select(cp => CartProductMap.Map(cp))
                .ToList();

            return new(
                checkoutCart.Id.Value,
                checkoutCart.CustomerId.Value,
                productsDto,
                checkoutCart.PaymentType,
                checkoutCart.PickupLocation,
                checkoutCart.TotalPrice);
        }
    }
}
