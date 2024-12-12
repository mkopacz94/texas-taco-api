using Asp.Versioning;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Mapping;
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

                var productsDtos = products
                    .Select(p => ProductMap.Map(p))
                    .ToList();

                return Results.Ok(productsDtos);
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<ProductDto>));
        }
    }
}
