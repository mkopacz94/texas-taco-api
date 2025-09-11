using Asp.Versioning;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Products.Api.Endpoints.Categories
{
    public class DeleteCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("categories/{id}", [AuthorizeRole(Role.Admin)] async (
                string id,
                ICategoriesRepository categoriesRepository) =>
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    throw new InvalidRequestParametersException(
                        $"Given category ID ({id}) is not a valid GUID.");
                }

                var categoryId = new CategoryId(idGuid);
                await categoriesRepository.DeleteAsync(categoryId);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags(Tags.Categories)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
