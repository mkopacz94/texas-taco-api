using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class GetProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("{id}", async (
                string id,
                [FromServices] IProductsRepository productsRepository) =>
            {
                var productId = new ProductId(Guid.Parse(id));

                var product = await productsRepository
                    .GetAsync(productId)
                    ?? throw new ProductNotFoundException(productId);

                var productDto = ProductMap.Map(product);

                return Results.Ok(productDto);
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .WithName(Routes.GetProduct)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(ProductDto));
        }
    }
}
