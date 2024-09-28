
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Errors;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class UpdatePrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("prizes/{id}", async (
                string id,
                PrizeInputDto prizeDto,
                [FromServices] IPrizesRepository prizesRepository) =>
            {
                if (!Guid.TryParse(id, out var prizeIdGuid))
                {
                    var error = InvalidGuidErrorMessage.Create(id, "prize");
                    return Results.BadRequest(error.Message);
                }

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

                var prizeToUpdate = await prizesRepository
                    .GetAsync(new PrizeId(prizeIdGuid));

                if (prizeToUpdate is null)
                {
                    var errorMessage = new ErrorMessage(
                        Errors.ErrorsCodes.PrizeNotFound,
                        $"Prize with the \"{id}\" id has not been found in the database. " +
                            "Provide an id of an existing product.");

                    return Results.NotFound(errorMessage);
                }

                prizeToUpdate.UpdatePrize(
                    prizeDto.Name,
                    prizeDto.RequiredPointsAmount,
                    new ProductId(productIdGuid),
                    new PictureId(pictureIdGuid));

                await prizesRepository.UpdateAsync(prizeToUpdate);

                return Results.NoContent();
            })
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound, typeof(ErrorMessage));
        }
    }
}
