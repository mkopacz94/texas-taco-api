
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class UpdatePrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("prizes/{id}", [AuthorizeRole(Role.Admin)] async (
                string id,
                PrizeInputDto prizeDto,
                [FromServices] IPrizesRepository prizesRepository,
                [FromServices] IProductsRepository productsRepository,
                [FromServices] IPicturesRepository picturesRepository) =>
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

                var productId = new ProductId(productIdGuid);
                var pictureId = new PictureId(pictureIdGuid);

                await ValidateAssociatedProductAndPictureExistInDatabase(
                    productsRepository,
                    picturesRepository,
                    productId,
                    pictureId);

                prizeToUpdate.UpdatePrize(
                    prizeDto.Name,
                    prizeDto.RequiredPointsAmount,
                    productId,
                    pictureId);

                await prizesRepository.UpdateAsync(prizeToUpdate);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound, typeof(ErrorMessage));
        }

        private async static Task ValidateAssociatedProductAndPictureExistInDatabase(
            IProductsRepository productsRepository,
            IPicturesRepository picturesRepository,
            ProductId productId,
            PictureId pictureId)
        {
            if (!await productsRepository.AnyAsync(productId))
            {
                throw new ProductNotFoundException(productId);
            }

            if (!await picturesRepository.AnyAsync(pictureId))
            {
                throw new PictureNotFoundException(pictureId);
            }
        }
    }
}
