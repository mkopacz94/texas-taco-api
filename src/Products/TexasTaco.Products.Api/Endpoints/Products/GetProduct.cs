using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;

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
                var product = await productsRepository
                    .GetAsync(new ProductId(Guid.Parse(id)));

                return Results.Ok(product);
            })
            .WithTags(Tags.Products)
            .WithName(Routes.GetProduct)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(Product));
        }
    }
}
