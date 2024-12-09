﻿using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Cart
{
    public class Cart(CustomerId customerId)
    {
        private const int MaximumAmountOfProductInCart = 5;

        private readonly List<CartProduct> _products = [];

        public CartId Id { get; } = new(Guid.NewGuid());
        public CustomerId CustomerId { get; private set; } = customerId;
        public IReadOnlyCollection<CartProduct> Products => _products;

        public void AddProduct(CartProduct product)
        {
            if (product.Quantity > MaximumAmountOfProductInCart)
            {
                throw new ProductAmountExceededException(product, MaximumAmountOfProductInCart);
            }

            var sameItemInCart = _products.SingleOrDefault(i => i.ProductId == product.ProductId);

            if (sameItemInCart is null)
            {
                _products.Add(product);
                return;
            }

            if (sameItemInCart.Quantity + product.Quantity > MaximumAmountOfProductInCart)
            {
                throw new ProductAmountExceededException(product, MaximumAmountOfProductInCart);
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
    }
}
