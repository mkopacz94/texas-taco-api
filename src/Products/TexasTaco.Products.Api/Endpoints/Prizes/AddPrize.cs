using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class AddPrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("prizes", async (
                [FromServices] IPrizesRepository prizesRepository, 
                [FromBody] AddPrizeDto addPrizeDto) =>
            {
                var prizeToAdd = new Prize(
                    addPrizeDto.Name,
                    addPrizeDto.RequiredPointsAmount,
                    new ProductId(Guid.Parse(addPrizeDto.ProductId)),
                    new PictureId(Guid.Parse(addPrizeDto.PictureId)));

                await prizesRepository.AddAsync(prizeToAdd);
                return Results.CreatedAtRoute(Routes.GetPrize, new { Id = prizeToAdd.Id.Value });
            })
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Prize));
        }
    }
}
