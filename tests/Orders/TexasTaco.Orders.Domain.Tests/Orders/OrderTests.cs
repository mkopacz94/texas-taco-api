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
    }
}
