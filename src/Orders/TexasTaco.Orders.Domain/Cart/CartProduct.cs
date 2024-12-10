using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Cart
{
    public class CartProduct
    {
        public CartProductId Id { get; } = new(Guid.NewGuid());
        public ProductId ProductId { get; private set; }
        public CartId CartId { get; private set; } = null!;
        public Cart Cart { get; private set; } = null!;
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string? PictureUrl { get; private set; }
        public int Quantity { get; private set; }

        public CartProduct(
            ProductId productId,
            string name,
            decimal price,
            string? pictureUrl,
            int quantity)
        {
            if (quantity < 1)
            {
                throw new InvalidCartProductQuantityException(quantity);
            }

            ProductId = productId;
            Name = name;
            Price = price;
            PictureUrl = pictureUrl;
            Quantity = quantity;
        }

        public void ChangeQuantity(int quantity) => Quantity = quantity;
        public void IncreaseQuantity(int quantity) => Quantity += quantity;
        public void UpdatePrice(decimal newPrice) => Price = newPrice;
    }
}
