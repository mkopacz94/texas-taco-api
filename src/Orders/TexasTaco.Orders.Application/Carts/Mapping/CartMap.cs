using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.Mapping
{
    internal static class CartMap
    {
        public static CartDto Map(Cart cart)
        {
            var productsDto = cart
                .Products
                .Select(cp => CartProductMap.Map(cp))
                .ToList();

            return new(
                cart.Id.Value,
                cart.CustomerId.Value,
                productsDto,
                cart.TotalPrice);
        }
    }
}
