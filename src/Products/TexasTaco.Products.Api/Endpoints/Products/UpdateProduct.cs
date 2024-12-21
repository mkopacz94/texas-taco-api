using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Services;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class UpdateProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("{id}", [AuthorizeRole(Role.Admin)] async (
                string id,
                ProductInputDto productDto,
                [FromServices] IProductUpdateService productUpdateService) =>
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

                var productId = new ProductId(Guid.Parse(id));
                await productUpdateService.UpdateProductAsync(productId, productDto);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorMessage));
        }
    }
}
