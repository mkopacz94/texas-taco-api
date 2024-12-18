using FluentAssertions;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Domain.Tests.Orders
{
    public class OrderTests
    {
        [Fact]
        public void CollectOrderId_Should_ReturnFirstFiveCharactersFromIdGuid()
        {
            //Arrange
            var cart = new Domain.Cart.Cart(new CustomerId(Guid.NewGuid()));
            var product = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                10,
                null,
                1);

            cart.AddProduct(product);
            var checkoutCart = cart.Checkout();

            //Act
            var order = checkoutCart.PlaceOrder();

            //Assert
            order
                .CollectOrderId
                .Should()
                .Be(order.Id.Value.ToString()[..5]);
        }

        [Theory]
        [InlineData(10.99, 2, 5.25, 4, 430)]
        [InlineData(2.1, 1, 3.8, 1, 59)]
        public void CalculatePoints_Should_Calculate10PointsForEveryZlotySpentAndCeilResult(
            decimal product1Price, int product1Quantity,
            decimal product2Price, int product2Quantity,
            int expectedPoints)
        {
            var cart = new Domain.Cart.Cart(new CustomerId(Guid.NewGuid()));

            var product1 = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                product1Price,
                null,
                product1Quantity);

            var product2 = new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Product",
                product2Price,
                null,
                product2Quantity);


            cart.AddProduct(product1);
            cart.AddProduct(product2);
            var checkoutCart = cart.Checkout();

            //Act
            var order = checkoutCart.PlaceOrder();

            //Assert
            order.PointsCollected
                .Should()
                .Be(expectedPoints);
        }
    }
}
