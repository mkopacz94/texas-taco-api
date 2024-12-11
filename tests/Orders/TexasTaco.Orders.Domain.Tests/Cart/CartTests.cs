using FluentAssertions;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Tests.Cart
{
    public class CartTests
    {
        private readonly CustomerId _customerId = new(Guid.NewGuid());

        [Fact]
        public void AddProduct_Should_AddProductsWithCorrectQuantityToCart()
        {
            //Arrange
            var cart = new Domain.Cart.Cart(_customerId);
            var firstProductGuid = Guid.NewGuid();
            var secondProductGuid = Guid.NewGuid();

            var firstProduct = new CartProduct(
                new ProductId(firstProductGuid),
                "Product",
                10,
                null,
                3);

            var secondProduct = new CartProduct(
                new ProductId(secondProductGuid),
                "Product",
                10,
                null,
                4);

            //Act
            cart.AddProduct(firstProduct);
            cart.AddProduct(secondProduct);

            //Assert
            cart.Products
                .Should()
                .OnlyContain(p => p.ProductId.Value == firstProductGuid
                    || p.ProductId.Value == secondProductGuid);

            cart.Products
                .Should()
                .Contain(p => p.ProductId.Value == firstProductGuid
                    && p.Quantity == firstProduct.Quantity);

            cart.Products
                .Should()
                .Contain(p => p.ProductId.Value == secondProductGuid
                    && p.Quantity == secondProduct.Quantity);
        }

        [Fact]
        public void AddProduct_Should_ThrowTooManyProductsInCartException_IfExceededMaximumAmountOfProducts()
        {
            //Arrange
            var cart = new Domain.Cart.Cart(_customerId);

            var firstProduct = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                5);

            var secondProduct = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                5);

            var thirdProduct = new CartProduct(
               new ProductId(Guid.NewGuid()),
               "Product",
               10,
               null,
               3);

            var quantityToAdd = Domain.Cart.Cart.MaximumAmountOfProducts
                - firstProduct.Quantity
                - secondProduct.Quantity
                - thirdProduct.Quantity
                + 1;

            var fourthProduct = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                quantityToAdd);

            //Act
            cart.AddProduct(firstProduct);
            cart.AddProduct(secondProduct);
            cart.AddProduct(thirdProduct);

            Action addAction = () => cart.AddProduct(fourthProduct);

            //Assert
            addAction
                .Should()
                .Throw<TooManyProductsInCartException>();
        }
    }
}
