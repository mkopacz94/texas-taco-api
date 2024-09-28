using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Errors;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class AddPrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("prizes", async (
                [FromServices] IPrizesRepository prizesRepository, 
                [FromBody] PrizeInputDto prizeDto) =>
            {
                if (!Guid.TryParse(prizeDto.ProductId, out var productIdGuid))
                {
                    var error = InvalidGuidErrorMessage.Create(prizeDto.ProductId, "product");
                    return Results.BadRequest(error.Message);
                }

                if (!Guid.TryParse(prizeDto.PictureId, out var pictureIdGuid))
                {
                    var error = InvalidGuidErrorMessage.Create(prizeDto.PictureId, "picture");
                    return Results.BadRequest(error.Message);
                }

                var prizeToAdd = new Prize(
                    prizeDto.Name,
                    prizeDto.RequiredPointsAmount,
                    new ProductId(productIdGuid),
                    new PictureId(pictureIdGuid));

                await prizesRepository.AddAsync(prizeToAdd);
                return Results.CreatedAtRoute(Routes.GetPrize, new { Id = prizeToAdd.Id.Value });
            })
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Prize));
        }
    }
}
