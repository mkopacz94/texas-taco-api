using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.Mapping
{
    internal static class CartProductMap
    {
        public static CartProductDto Map(CartProduct product)
        {
            return new(
                product.Id.Value,
                product.ProductId.Value,
                product.Name,
                product.Price,
                product.PictureUrl,
                product.Quantity);
        }
    }
}
