
using Asp.Versioning;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    public class DeleteProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("{id}", [AuthorizeRole(Role.Admin)] async (
                string id,
                IProductsRepository productsRepository) =>
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    throw new InvalidRequestParametersException(
                        $"Given product ID ({id}) is not a valid GUID.");
                }

                var productId = new ProductId(idGuid);
                await productsRepository.DeleteAsync(productId);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
