using System.Net.Http.Json;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Api.Tests.Controllers
{
    public class OrdersControllerTests(IntegrationTestWebAppFactory factory)
        : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GetCustomerOrder_Should_ReturnCustomerOrder()
        {
            //Arrange
            var customer = new Customer(
                new AccountId(Guid.NewGuid()),
                new EmailAddress("test@email.com"));

            var cart = new Cart(customer.Id);
            cart.AddProduct(new CartProduct(
                new ProductId(Guid.NewGuid()),
                "Test product",
                25.99m,
                "picture_url",
                3));

            var checkoutCart = new CheckoutCart(cart);
            var order = checkoutCart.PlaceOrder();

            DbContext.Customers.Add(customer);
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();

            //Act
            var response = await Client
                .GetAsync($"/api/v1/orders?customerId={customer.Id.Value}");

            var customerOrder = await response
                .Content
                .ReadFromJsonAsync<OrderDto>();

            //Assert
        }
    }
}
