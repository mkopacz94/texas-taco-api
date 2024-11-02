using TexasTaco.Orders.Domain.Basket.Exceptions;
using TexasTaco.Orders.Domain.Customer;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Basket
{
    public class Basket(CustomerId customerId)
    {
        private readonly List<BasketItem> _items = [];

        public BasketId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; } = customerId;
        public IReadOnlyCollection<BasketItem> Items => _items;

        public void AddProduct(
            ProductId productId,
            string name,
            decimal price,
            string pictureUrl)
        {
            var basketItem = _items.SingleOrDefault(i => i.ProductId == productId);

            if (basketItem is not null)
            {
                basketItem.IncreaseQuantity();
                return;
            }

            var newItem = new BasketItem(productId, name, price, pictureUrl, 1);

            _items.Add(newItem);
        }

        public void RemoveItem(ProductId productId)
        {
            var basketItem = _items.SingleOrDefault(i => i.ProductId == productId)
                ?? throw new BasketItemNotFoundException(productId);

            basketItem.DecreaseQuantity();
        }

        public void Clear() => _items.Clear();
    }
}
