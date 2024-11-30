using TexasTaco.Orders.Domain.Basket.Exceptions;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Basket
{
    public class Basket(CustomerId customerId)
    {
        private readonly List<BasketItem> _items = [];

        public BasketId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; } = customerId;
        public IReadOnlyCollection<BasketItem> Items => _items;

        public void AddProduct(BasketItem item)
        {
            var sameItemInBasket = _items.SingleOrDefault(i => i.ProductId == item.ProductId);

            if (sameItemInBasket is not null)
            {
                sameItemInBasket.IncreaseQuantity(item.Quantity);
                return;
            }

            _items.Add(item);
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
