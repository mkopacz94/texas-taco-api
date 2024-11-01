using Asp.Versioning;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class GetAllProducts : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("", async (IProductsRepository productsRepository) =>
            {
                var products = await productsRepository.GetAllAsync();
                return Results.Ok(products);
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<Product>));
        }
    }
}
