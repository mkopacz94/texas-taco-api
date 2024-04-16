using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using TexasTaco.Authentication.Api.Services;

namespace TexasTaco.Authentication.Api.Tests.Services
{
    public class CookieServiceTests
    {
        private readonly IHttpContextAccessor _httpContextAccessorMock
            = Substitute.For<IHttpContextAccessor>();

        [Fact]
        public void GetCookie_Should_ReturnNull_IfCookieNotExist()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();

            _httpContextAccessorMock.HttpContext
                .Returns(httpContext);

            var cookieService = new CookieService(_httpContextAccessorMock);

            //Act
            string? cookieValue = cookieService.GetCookie("cookie");

            //Assert
            cookieValue
                .Should()
                .BeNull();
        }

        [Fact]
        public void GetCookie_Should_ReturnCookieValue_IfCookieExists()
        {
            //Arrange
            string cookieName = "cookie";
            string expectedCookieValue = "cookieValue";

            var cookieCollectionMock = new FakeCookieCollection
            {
                { cookieName, expectedCookieValue }
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Cookies = cookieCollectionMock;

            _httpContextAccessorMock.HttpContext
                .Returns(httpContext);

            var cookieService = new CookieService(_httpContextAccessorMock);

            //Act
            string? cookieValue = cookieService.GetCookie(cookieName);

            //Assert
            cookieValue
                .Should()
                .Be(expectedCookieValue);
        }

        [Fact]
        public void SetCookie_Should_SetCookieWithOptionsInHttpContextResponse()
        {
            //Arrange
            string cookieName = "cookie";
            string cookieValue = "cookieValue";

            var httpContext = new DefaultHttpContext();

            _httpContextAccessorMock.HttpContext
                .Returns(httpContext);

            var cookieService = new CookieService(_httpContextAccessorMock);

            //Act
            var cookieOptions = new CookieOptions
            {
                Domain = "test",
                Expires = new DateTime(2024, 4, 11)
            };

            cookieService.SetCookie(cookieName, cookieValue, cookieOptions);

            //Assert
            httpContext.Response.Headers.SetCookie[0]
                .Should()
                .Be("cookie=cookieValue; expires=Wed, 10 Apr 2024 22:00:00 GMT; domain=test; path=/");
        }

        internal class FakeCookieCollection : Dictionary<string, string>, IRequestCookieCollection
        {
            ICollection<string> IRequestCookieCollection.Keys => Keys;
        }
    }
}
