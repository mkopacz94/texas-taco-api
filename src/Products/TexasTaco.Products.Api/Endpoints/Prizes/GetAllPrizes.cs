using Asp.Versioning;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class GetAllPrizes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("prizes", async (IPrizesRepository prizesRepository) =>
            {
                var prizes = await prizesRepository.GetAllAsync();
                var prizesDtos = prizes
                    .Select(p => PrizeMap.Map(p))
                    .ToList();

                return Results.Ok(prizesDtos);
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<PrizeDto>));
        }
    }
}
