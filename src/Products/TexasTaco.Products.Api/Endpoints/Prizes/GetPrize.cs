using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.Mapping;
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
                var prizeId = new PrizeId(Guid.Parse(id));

                var prize = await prizesRepository
                    .GetAsync(prizeId)
                    ?? throw new PrizeNotFoundException(prizeId);

                var prizeDto = PrizeMap.Map(prize);
                return Results.Ok(prizeDto);
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .WithName(Routes.GetPrize)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(PrizeDto));
        }
    }
}
