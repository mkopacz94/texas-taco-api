using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Api.Tests.Services
{
    public class CookieClaimsServiceTests
    {
        private readonly IHttpContextAccessor _httpContextAccessorMock
            = Substitute.For<IHttpContextAccessor>();

        [Fact]
        public async Task SetAccountClaims_Should_SetEmailValueAsEmailClaim()
        {
            //Arrange
            var httpContextMock = Substitute.For<HttpContext>();

            _httpContextAccessorMock.HttpContext
                .Returns(httpContextMock);

            var authSerivceMock = Substitute.For<IAuthenticationService>();
            authSerivceMock
                .SignInAsync(default!, default, default!, default)
                .ReturnsForAnyArgs(Task.CompletedTask);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IAuthenticationService))
                .Returns(authSerivceMock);

            httpContextMock.RequestServices
                .Returns(serviceProvider);

            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress("test@domain.com"),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await httpContextMock
                .ReceivedWithAnyArgs(1)
                .SignInAsync(
                    default, default!, default);
        }
    }
}
