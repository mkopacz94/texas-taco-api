using Asp.Versioning;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Products.Api.Endpoints.Categories
{
    internal class GetCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories/{id}", async (
                string id,
                ICategoriesRepository categoriesRepository) =>
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    throw new InvalidRequestParametersException(
                        $"Given category ID ({id}) is not a valid GUID.");
                }

                var categoryId = new CategoryId(idGuid);

                var category = await categoriesRepository
                    .GetAsync(categoryId)
                    ?? throw new CategoryNotFoundException(categoryId);

                var categoryDto = CategoryMap.Map(category);

                return Results.Ok(categoryDto);
            })
            .RequireAuthorization()
            .WithTags(Tags.Categories)
            .WithName(Routes.GetCategory)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(CategoryDto));
        }
    }
}
