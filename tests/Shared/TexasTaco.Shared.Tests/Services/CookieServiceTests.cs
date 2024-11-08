using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
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
            var cookieExpirationTimeInUtc = DateTime.UtcNow;

            //Act
            var cookieOptions = new CookieOptions
            {
                Domain = "test",
                Expires = cookieExpirationTimeInUtc,
                SameSite = SameSiteMode.Lax
            };

            cookieService.SetCookie(cookieName, cookieValue, cookieOptions);

            //Assert
            string expectedCookieValue = $"cookie=cookieValue; " +
                $"expires={cookieExpirationTimeInUtc:R}; domain=test; path=/; samesite=lax";

            httpContext.Response.Headers
                .Should()
                .Contain(h => h.Value == expectedCookieValue);
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
