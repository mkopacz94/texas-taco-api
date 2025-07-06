using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using TexasTaco.Orders.Api.Tests.Base;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Application.Customers.DTO;

namespace TexasTaco.Orders.Api.Tests.Tests
{
    public class CustomersTests(
        IntegrationTestWebAppFactory factory)
        : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GetCustomer_Should_ReturnCustomer()
        {
            //Arrange
            var customer = await DbContext
                .Customers
                .FirstAsync();

            //Act
            var fetchCustomerResponse = await HttpClient
                .GetAsync(
                    $"api/v1/orders/customers/?accountId={customer.AccountId.Value}");

            fetchCustomerResponse.EnsureSuccessStatusCode();

            var fetchedCustomer = await fetchCustomerResponse
                .Content
                .ReadFromJsonAsync<CustomerDto>();

            //Assert
            fetchedCustomer.ShouldNotBeNull();

            fetchedCustomer.FirstName.ShouldBe(customer.FirstName);
            fetchedCustomer.EmailAddress.ShouldBe(customer.Email.Value);
        }

        [Fact]
        public async Task GetCustomer_Should_ReturnNotFound_IfCustomerDoesNotExist()
        {
            //Arrange
            var notExistingAccountId = Guid.NewGuid();

            //Act
            var fetchCustomerResponse = await HttpClient
                .GetAsync(
                    $"api/v1/orders/customers/?accountId={notExistingAccountId}");

            //Assert
            fetchCustomerResponse
                .StatusCode
                .ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
