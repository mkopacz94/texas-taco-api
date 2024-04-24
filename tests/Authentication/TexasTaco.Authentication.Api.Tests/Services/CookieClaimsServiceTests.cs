using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Api.Tests.Services
{
    public class CookieClaimsServiceTests
    {
        private readonly HttpContext _httpContextMock
            = Substitute.For<HttpContext>();

        private readonly IHttpContextAccessor _httpContextAccessorMock
            = Substitute.For<IHttpContextAccessor>();

        private readonly IAuthenticationService _authenticationServiceMock
            = Substitute.For<IAuthenticationService>();

        public CookieClaimsServiceTests()
        {
            _httpContextAccessorMock.HttpContext
                .Returns(_httpContextMock);

            _authenticationServiceMock
                .SignInAsync(default!, default, default!, default)
                .ReturnsForAnyArgs(Task.CompletedTask);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IAuthenticationService))
                .Returns(_authenticationServiceMock);

            _httpContextMock.RequestServices
                .Returns(serviceProvider);
        }

        [Fact]
        public async Task SetAccountClaims_Should_SignInWithAllRequiredClaimsTypes()
        {
            //Arrange
            List<string> requiredClaimsTypes = [
                ClaimTypes.Email,
                TexasTacoClaimNames.AccountId,
                ClaimTypes.Role
            ];

            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress("test@domain.com"),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await _authenticationServiceMock
                .Received(1)
                .SignInAsync(
                    _httpContextMock,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    Arg.Is<ClaimsPrincipal>(p => p.Claims
                        .All(c => requiredClaimsTypes.Contains(c.Type ))),
                    Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task SetAccountClaims_Should_SignInWithEmailValueAsEmailClaim()
        {
            //Arrange
            string email = "test@domain.com";

            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress(email),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await _authenticationServiceMock
                .Received(1)
                .SignInAsync(
                    _httpContextMock, 
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    Arg.Is<ClaimsPrincipal>(p => p.Claims
                        .Any(c => c.Type == ClaimTypes.Email && c.Value == email)), 
                    Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task SetAccountClaims_Should_SignInWithAccountIdValueAsAccountIdClaim()
        {
            //Arrange
            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress("test@domain.com"),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await _authenticationServiceMock
                .Received(1)
                .SignInAsync(
                    _httpContextMock,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    Arg.Is<ClaimsPrincipal>(p => p.Claims
                        .Any(c => c.Type == TexasTacoClaimNames.AccountId && c.Value == account.Id.Value.ToString())),
                    Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task SetAccountClaims_Should_SignInWithRoleStringAsRoleClaim()
        {
            //Arrange
            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress("test@domain.com"),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await _authenticationServiceMock
                .Received(1)
                .SignInAsync(
                    _httpContextMock,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    Arg.Is<ClaimsPrincipal>(p => p.Claims
                        .Any(c => c.Type == ClaimTypes.Role && c.Value == account.Role.ToString())),
                    Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task SetAccountClaims_Should_SignInWithIsPersistentAuthenticationProperties()
        {
            //Arrange
            var claimsService = new CookieClaimsService(_httpContextAccessorMock);

            var account = new Account(
                new EmailAddress("test@domain.com"),
                Role.Customer,
                [],
                []);

            //Act
            await claimsService.SetAccountClaims(account);

            //Assert
            await _authenticationServiceMock
                .Received(1)
                .SignInAsync(
                    _httpContextMock,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    Arg.Any<ClaimsPrincipal>(),
                    Arg.Is<AuthenticationProperties>(p => p.IsPersistent));
        }
    }
}
