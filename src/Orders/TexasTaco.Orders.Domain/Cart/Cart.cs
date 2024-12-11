using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Cart
{
    public sealed class Cart(CustomerId customerId)
    {
        public const int MaximumAmountOfProducts = 15;

        private readonly List<CartProduct> _products = [];

        public CartId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; } = customerId;
        public CheckoutCart? CheckoutCart { get; private set; } = null;
        public IReadOnlyCollection<CartProduct> Products => _products;

        public void AddProduct(CartProduct product)
        {
            if (Products.Sum(p => p.Quantity) + product.Quantity > MaximumAmountOfProducts)
            {
                throw new TooManyProductsInCartException(Id, MaximumAmountOfProducts);
            }

            var sameItemInCart = _products.SingleOrDefault(i => i.ProductId == product.ProductId);

            if (sameItemInCart is null)
            {
                _products.Add(product);
                return;
            }

            sameItemInCart.IncreaseQuantity(product.Quantity);
        }

        public void RemoveItem(CartProductId id)
        {
            var cartProduct = _products.SingleOrDefault(i => i.Id == id)
                ?? throw new CartProductNotFoundException(id);

            _products.Remove(cartProduct);
        }

        public void Clear() => _products.Clear();

        public bool ContainsProduct(ProductId productId)
            => Products.Any(i => i.ProductId == productId);

        public CheckoutCart Checkout()
        {
            if (Products.Count == 0)
            {
                throw new CannotCheckoutEmptyCartException();
            }

            return new CheckoutCart(this);
        }
    }
}
