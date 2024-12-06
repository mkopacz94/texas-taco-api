using TexasTaco.Orders.Domain.Basket.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Basket
{
    public class BasketItem
    {
        public BasketItemId Id { get; } = new(Guid.NewGuid());
        public ProductId ProductId { get; private set; }
        public BasketId BasketId { get; private set; } = null!;
        public Basket Basket { get; private set; } = null!;
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string? PictureUrl { get; private set; }
        public int Quantity { get; private set; }

        public BasketItem(
            ProductId productId,
            string name,
            decimal price,
            string? pictureUrl,
            int quantity)
        {
            if (quantity < 1)
            {
                throw new InvalidBasketItemQuantityException(quantity);
            }

            ProductId = productId;
            Name = name;
            Price = price;
            PictureUrl = pictureUrl;
            Quantity = quantity;
        }

        public void ChangeQuantity(int quantity) => Quantity = quantity;
        public void IncreaseQuantity(int quantity) => Quantity += quantity;
        public void DecreaseQuantity()
        {
            int decreasedQuantity = Quantity - 1;

            if (decreasedQuantity < 0)
            {
                throw new InvalidBasketItemQuantityException(decreasedQuantity);
            }

            Quantity = decreasedQuantity;
        }
    }
}
