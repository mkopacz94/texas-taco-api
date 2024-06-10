using Asp.Versioning;
using TexasTaco.Products.Core.Entities;
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
                return Results.Ok(prizes);
            })
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<Prize>));
        }
    }
}
