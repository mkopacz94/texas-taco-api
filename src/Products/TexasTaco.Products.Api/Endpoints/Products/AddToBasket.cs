using Asp.Versioning;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class AddToBasket : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("{id}", (string id) =>
            {
                return Results.CreatedAtRoute("Basket", new { Id = id });
            })
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Product));
        }
    }
}
