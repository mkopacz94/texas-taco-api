using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Cart
{
    public sealed class CartProduct
    {
        public const int MaximumAmountOfProduct = 5;

        public CartProductId Id { get; } = new(Guid.NewGuid());
        public ProductId ProductId { get; private set; }
        public CartId CartId { get; private set; } = null!;
        public Cart Cart { get; private set; } = null!;
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string? PictureUrl { get; private set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (value < 0)
                {
                    throw new InvalidCartProductQuantityException(value);
                }

                if (value > MaximumAmountOfProduct)
                {
                    throw new ProductAmountExceededException(this, MaximumAmountOfProduct);
                }

                _quantity = value;
            }
        }

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

        public void IncreaseQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new InvalidCartProductQuantityException(quantity);
            }

            Quantity += quantity;
        }

        public void UpdatePrice(decimal newPrice) => Price = newPrice;
    }
}
