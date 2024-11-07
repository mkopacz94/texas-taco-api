using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class UpdateProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("{id}", async (
                string id,
                ProductInputDto productDto,
                [FromServices] IProductsRepository productsRepository) =>
            {
                if (!Guid.TryParse(id, out var productIdGuid))
                {
                    var errorMessage = InvalidGuidErrorMessage.Create(id, "product");
                    return Results.BadRequest(errorMessage);
                }

                if (!Guid.TryParse(productDto.PictureId, out var pictureIdGuid))
                {
                    var errorMessage = InvalidGuidErrorMessage.Create(productDto.PictureId, "picture");
                    return Results.BadRequest(errorMessage);
                }

                var productToUpdate = await productsRepository
                    .GetAsync(new ProductId(Guid.Parse(id)));

                if (productToUpdate is null)
                {
                    var errorMessage = new ErrorMessage(
                        Errors.ErrorsCodes.ProductNotFound,
                        $"Product with the \"{id}\" id has not been found in the database. " +
                            "Provide an id of an existing product.");

                    return Results.NotFound(errorMessage);
                }

                productToUpdate.UpdateProduct(
                    productDto.Name,
                    productDto.ShortDescription,
                    productDto.Recommended,
                    productDto.Price,
                    new PictureId(pictureIdGuid));

                await productsRepository.UpdateAsync(productToUpdate);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound, typeof(ErrorMessage));
        }
    }
}
