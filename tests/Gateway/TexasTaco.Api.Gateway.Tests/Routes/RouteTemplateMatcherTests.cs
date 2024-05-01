using FluentAssertions;
using TexasTaco.Api.Gateway.Routes;

namespace TexasTaco.Api.Gateway.Tests.Routes
{
    public class RouteTemplateMatcherTests
    {
        [Theory]
        [InlineData("/api/test/123", "/api/test/123", true)]
        [InlineData("/api/{version}/test/123", "/api/v1/test/123", true)]
        [InlineData("/api/{version}/test/123", "/api/v20/test/123", true)]
        [InlineData("/api/{version}/test/123", "/api/v1.4/test/123", true)]
        [InlineData("/api/{version}/test/123", "/api/v11.24.1/test/123", true)]
        [InlineData("/api/test/123", "/api/tes/123", false)]
        [InlineData("/api/test/123", "/api/123", false)]
        [InlineData("/api/test/123", "/domain/api/test/123", true)]
        public void RouteEndMatchesTemplate_Should_ReturnCorrectBooleanValue(
            string template, string routeToCheck, bool expectedMatchResult)
        {
            //Arrange / Act
            bool matches = RouteTemplateMatcher
                .RouteEndMatchesTemplate(routeToCheck, template);

            //Assert
            matches
                .Should()
                .Be(expectedMatchResult);
        }
    }
}
