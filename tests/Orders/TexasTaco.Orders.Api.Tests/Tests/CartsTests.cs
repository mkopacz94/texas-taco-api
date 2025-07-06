using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using TexasTaco.Orders.Api.Tests.Base;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Application.Carts.DTO;

namespace TexasTaco.Orders.Api.Tests.Tests
{
    public class CartsTests(
        IntegrationTestWebAppFactory factory)
        : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GetCart_Should_ReturnCartWithProducts()
        {
            //Arrange
            var customer = await DbContext
                .Customers
                .FirstAsync();

            var cartInDb = await DbContext
                .Carts
                .FirstAsync(c => c.CustomerId == customer.Id);

            //Act
            var cartResponse = await HttpClient
                .GetAsync($"api/v1/orders/carts?customerId={customer.Id.Value}");

            cartResponse.EnsureSuccessStatusCode();

            var cartDto = await cartResponse
                .Content
                .ReadFromJsonAsync<CartDto>();

            //Assert
            cartDto.ShouldNotBeNull();
            cartDto.CustomerId.ShouldBe(customer.Id.Value);
            cartDto.Products.Count.ShouldBe(cartInDb.Products.Count);
            cartDto.TotalPrice.ShouldBe(cartInDb.TotalPrice);

            cartDto.Products.ShouldAllBe(c =>
                cartInDb.Products.Any(p => p.Name == c.Name
                    && p.Quantity == c.Quantity));
        }

        [Fact]
        public async Task UpdateProductQuantity_Then_GetCart_Should_ReturnCartWithUpdatedProductQuantity()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            var productToUpdate = cart.Products.First();

            int newProductQuantity = 5;

            //Act
            var updateQuantityResponse = await HttpClient
                .PutAsync($"api/v1/orders/carts/{cart.Id.Value}" +
                    $"/products/{productToUpdate.Id.Value}" +
                    $"?quantity={newProductQuantity}", null);

            updateQuantityResponse.EnsureSuccessStatusCode();

            var cartResponse = await HttpClient
                .GetAsync($"api/v1/orders/carts?customerId={cart.CustomerId.Value}");

            cartResponse.EnsureSuccessStatusCode();

            var cartDto = await cartResponse
                .Content
                .ReadFromJsonAsync<CartDto>();

            //Assert
            cartDto.ShouldNotBeNull();

            cartDto
                .Products
                .First(p => p.Id == productToUpdate.Id.Value)
                .Quantity
                .ShouldBe(newProductQuantity);
        }

        [Fact]
        public async Task UpdateProductQuantity_Should_ReturnBadRequest_IfCartIdIsInvalid()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            var productToUpdate = cart.Products.First();

            //Act
            var updateQuantityResponse = await HttpClient
                .PutAsync($"api/v1/orders/carts/invalidCartId" +
                    $"/products/{productToUpdate.Id.Value}" +
                    $"?quantity=5", null);

            //Assert
            updateQuantityResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateProductQuantity_Should_ReturnBadRequest_IfProductIdIsInvalid()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            //Act
            var updateQuantityResponse = await HttpClient
                .PutAsync($"api/v1/orders/carts/{cart.Id.Value}" +
                    $"/products/invalidProductId" +
                    $"?quantity=5", null);

            //Assert
            updateQuantityResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task UpdateProductQuantity_Should_ReturnUnprocessableEntity_IfQuantityIsGreaterThan5(
            int newQuantity)
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            var productToUpdate = cart.Products.First();

            //Act
            var updateQuantityResponse = await HttpClient
                .PutAsync($"api/v1/orders/carts/{cart.Id.Value}" +
                    $"/products/{productToUpdate.Id.Value}" +
                    $"?quantity={newQuantity}", null);

            //Assert
            updateQuantityResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task RemoveProductFromCart_Then_GetCart_Should_ReturnCartWithRemovedProduct()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            var productToDelete = cart.Products.First();

            //Act
            var deleteProductResponse = await HttpClient
                .DeleteAsync($"api/v1/orders/carts/{cart.Id.Value}" +
                    $"/products/{productToDelete.Id.Value}");

            deleteProductResponse.EnsureSuccessStatusCode();

            var cartResponse = await HttpClient
                .GetAsync($"api/v1/orders/carts?customerId={cart.CustomerId.Value}");

            cartResponse.EnsureSuccessStatusCode();

            var cartDto = await cartResponse
                .Content
                .ReadFromJsonAsync<CartDto>();

            //Assert
            cartDto.ShouldNotBeNull();

            cartDto
                .Products
                .ShouldNotContain(p => p.Id == productToDelete.Id.Value);
        }

        [Fact]
        public async Task RemoveProductFromCart_Should_ReturnBadRequest_IfCartIdIsInvalid()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            var productToUpdate = cart.Products.First();

            //Act
            var updateQuantityResponse = await HttpClient
                .DeleteAsync($"api/v1/orders/carts/invalidCartId" +
                    $"/products/{productToUpdate.Id.Value}");

            //Assert
            updateQuantityResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RemoveProductFromCart_Should_ReturnBadRequest_IfProductIdIsInvalid()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            //Act
            var updateQuantityResponse = await HttpClient
                .DeleteAsync($"api/v1/orders/carts/{cart.Id.Value}" +
                    $"/products/invalidProductId");

            //Assert
            updateQuantityResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CheckoutCart_Then_GetCheckoutCart_Should_ReturnCheckoutCart()
        {
            //Arrange
            var cart = await DbContext
                .Carts
                .FirstAsync();

            //Act
            var checkoutCartResponse = await HttpClient
                .PostAsync(
                    $"api/v1/orders/carts/{cart.Id.Value}/checkout",
                    null);

            checkoutCartResponse.EnsureSuccessStatusCode();

            var createdCheckoutCartDto = await checkoutCartResponse
                .Content
                .ReadFromJsonAsync<CheckoutCartDto>();

            var getCheckoutCartResponse = await HttpClient
                .GetAsync(
                    $"api/v1/orders/checkout/{createdCheckoutCartDto!.Id}");

            getCheckoutCartResponse.EnsureSuccessStatusCode();

            var checkoutCartDto = await getCheckoutCartResponse
               .Content
               .ReadFromJsonAsync<CheckoutCartDto>();

            //Assert
            checkoutCartDto.ShouldNotBeNull();
            checkoutCartDto.Id.ShouldBe(createdCheckoutCartDto.Id);
        }

        [Fact]
        public async Task CheckoutCart_Should_ReturnBadRequest_IfCartIdIsInvalid()
        {
            //Arrange / Act
            var checkoutCartResponse = await HttpClient
                .PostAsync(
                    $"api/v1/orders/carts/invalidCartId/checkout",
                    null);

            //Assert
            checkoutCartResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
