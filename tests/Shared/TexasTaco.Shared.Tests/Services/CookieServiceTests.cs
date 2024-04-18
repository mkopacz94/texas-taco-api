using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Collections;
using TexasTaco.Shared.Services;

namespace TexasTaco.Shared.Tests.Services
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
            httpContext.Response.Headers
                .Should()
                .Contain(h => h.Value.Contains("cookie=cookieValue; expires=Wed, 10 Apr 2024 22:00:00 GMT; domain=test; path=/; samesite=lax"));
        }

        internal class FakeCookieCollection : Dictionary<string, string>, IRequestCookieCollection
        {
            ICollection<string> IRequestCookieCollection.Keys => Keys;

            IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
