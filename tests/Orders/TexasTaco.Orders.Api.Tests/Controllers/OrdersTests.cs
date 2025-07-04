using Shouldly;
using System.Net.Http.Json;
using TexasTaco.Orders.Api.Tests.Base;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Api.Tests.Controllers;

public class OrdersTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task PlaceOrderInCheckout_Then_ReadOrderStatus_Should_ReturnOrder()
    {
        // Arrange
        var customer = new Customer(
            new AccountId(Guid.NewGuid()),
            new EmailAddress("test@email.com"));

        var product = new CartProduct(
            new ProductId(Guid.NewGuid()),
            name: "Test product",
            price: 25.99m,
            pictureUrl: "picture_url",
            quantity: 3);

        var cart = new Cart(customer.Id);
        cart.AddProduct(product);
        var checkoutCart = new CheckoutCart(cart);

        DbContext.Customers.Add(customer);
        DbContext.Carts.Add(cart);
        DbContext.CheckoutCarts.Add(checkoutCart);
        await DbContext.SaveChangesAsync();

        // Act
        var placeOrderResponse = await HttpClient.PostAsync(
            $"/api/v1/orders/checkout/{checkoutCart.Id.Value}/place-order",
            content: null);

        placeOrderResponse.EnsureSuccessStatusCode();

        var createdOrder = await placeOrderResponse
            .Content
            .ReadFromJsonAsync<OrderDto>();

        var getOrderResponse = await HttpClient.GetAsync(
            $"/api/v1/orders/{createdOrder!.Id}");

        var fetchedOrder = await getOrderResponse
            .Content
            .ReadFromJsonAsync<OrderDto>();

        // Assert
        fetchedOrder.ShouldNotBeNull();
        fetchedOrder.Status.ShouldBe(OrderStatus.Placed);
        fetchedOrder.CustomerId.ShouldBe(customer.Id.Value);
        fetchedOrder.Lines.Count.ShouldBe(1);

        fetchedOrder.Lines.ShouldContain(line =>
            line.Name == product.Name &&
            line.Quantity == product.Quantity);

        fetchedOrder.PointsCollected.ShouldBeGreaterThan(0);
    }
}
