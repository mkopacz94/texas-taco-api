using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TexasTaco.Api.Gateway.Authentication;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Tests.Authentication
{
    public class TexasTacoAuthenticationMiddlewareTests
    {
        private const string NonAuthRequestPath = "/test/api";
        private const string AuthRequestPath = "/test/secret";
        private readonly DefaultHttpContext _httpContextMock = new();
        private readonly ICookieService _cookieServiceMock = Substitute.For<ICookieService>();
        private readonly IAuthenticationClient _authClientMock = Substitute.For<IAuthenticationClient>();


        [Fact]
        public async Task InvokeAsync_Should_CallNextFunction_IfRequestPathExistsInNonAuthenticationRoutes()
        {
            //Arrange
            bool nextFuncCalled = false;

            var routesConfiguration = CreateRoutesConfiguration(NonAuthRequestPath);

            var middleware = new TexasTacoAuthenticationMiddleware(
                _cookieServiceMock,
                _authClientMock,
                routesConfiguration);

            _httpContextMock.Request.Path = NonAuthRequestPath;

            //Act
            await middleware.InvokeAsync(_httpContextMock, () =>
            {
                nextFuncCalled = true;
                return Task.CompletedTask;
            });

            //Assert
            nextFuncCalled
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task InvokeAsync_Should_NotUpdateSessionCookie_IfRequestPathExistsInNonAuthenticationRoutes()
        {
            //Arrange
            var routesConfiguration = CreateRoutesConfiguration(NonAuthRequestPath);

            var middleware = new TexasTacoAuthenticationMiddleware(
                _cookieServiceMock,
                _authClientMock,
                routesConfiguration);

            _httpContextMock.Request.Path = NonAuthRequestPath;

            //Act
            await middleware.InvokeAsync(_httpContextMock, () =>
            {
                return Task.CompletedTask;
            });

            //Assert
            _cookieServiceMock
                .DidNotReceive()
                .SetCookie(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CookieOptions>());
        }

        [Fact]
        public async Task InvokeAsync_Should_UpdateSessionCookieAndCallNextFunc_IfAuthClientReturnedNotNullSessionAndRouteShouldBeAuthenticated()
        {
            //Arrange
            bool nextFuncCalled = false;

            var routesConfiguration = CreateRoutesConfiguration(NonAuthRequestPath);
            var sessionId = Guid.NewGuid();

            _authClientMock
                .GetSession(sessionId.ToString())
                .Returns(new Session(DateTime.UtcNow));

            var middleware = new TexasTacoAuthenticationMiddleware(
                _cookieServiceMock,
                _authClientMock,
                routesConfiguration);

            _httpContextMock.Request.Path = AuthRequestPath;
            _httpContextMock.Request.Cookies = MockRequestCookieCollection(
                "session_id", sessionId.ToString());

            //Act
            await middleware.InvokeAsync(_httpContextMock, () =>
            {
                nextFuncCalled = true;
                return Task.CompletedTask;
            });

            //Assert
            _cookieServiceMock
                .Received()
                .SetCookie(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CookieOptions>());

            nextFuncCalled
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task InvokeAsync_Should_SetUnauthorizedResultInHttpContextItems_IfAuthClientReturnedNullSession()
        {
            //Arrange
            var routesConfiguration = CreateRoutesConfiguration(NonAuthRequestPath);

            _authClientMock
                .GetSession(Arg.Any<string>())
                .ReturnsNull();

            var middleware = new TexasTacoAuthenticationMiddleware(
                _cookieServiceMock,
                _authClientMock,
                routesConfiguration);

            _httpContextMock.Request.Path = AuthRequestPath;

            //Act
            await middleware.InvokeAsync(_httpContextMock, () =>
            {
                return Task.CompletedTask;
            });

            //Assert
            var gatewayErrors = (ICollection<Error>)_httpContextMock.Items
                .First()
                .Value!;

            gatewayErrors
                .Should()
                .Contain(e => e.HttpStatusCode == StatusCodes.Status401Unauthorized);
        }

        private RoutesConfiguration CreateRoutesConfiguration(string routePath)
        {
            return new RoutesConfiguration
            {
                NonAuthenticationRoutes =
                [
                    new Route
                    {
                        Path = routePath
                    }
                ]
            };
        }

        private static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
        {
            var requestFeature = new HttpRequestFeature();
            var featureCollection = new FeatureCollection();

            requestFeature.Headers = new HeaderDictionary
            {
                { HeaderNames.Cookie, new StringValues(key + "=" + value) }
            };

            featureCollection.Set<IHttpRequestFeature>(requestFeature);

            var cookiesFeature = new RequestCookiesFeature(featureCollection);

            return cookiesFeature.Cookies;
        }
    }
}
