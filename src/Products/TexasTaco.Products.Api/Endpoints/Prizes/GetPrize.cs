using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class GetPrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("prizes/{id}", async (
                string id,
                [FromServices] IPrizesRepository prizesRepository) =>
            {
                var product = await prizesRepository
                    .GetAsync(new PrizeId(Guid.Parse(id)));

                return Results.Ok(product);
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .WithName(Routes.GetPrize)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(Prize));
        }
    }
}
