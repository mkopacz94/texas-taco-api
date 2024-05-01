using Asp.Versioning;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TexasTaco.Products.Api.Endpoints.Antiforgery
{
    internal class GetAntiforgeryToken : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("antiforgery/token",
                (IAntiforgery _antiforgeryService, HttpContext context) =>
            {
                var tokens = _antiforgeryService.GetAndStoreTokens(context);
                var xsrfToken = tokens.RequestToken!;
                return TypedResults.Content(xsrfToken, "text/plain");
            })
            .WithTags(Tags.Security)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(ContentHttpResult))
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
