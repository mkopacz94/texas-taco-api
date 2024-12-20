
using Asp.Versioning;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Api.Endpoints.Categories
{
    internal class GetCategories : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories", async (
                ICategoriesRepository categoriesRepository) =>
            {
                var categories = await categoriesRepository
                    .GetAllAsync();

                var categoriesDtos = categories
                    .Select(c => CategoryMap.Map(c));

                return Results.Ok(categoriesDtos);
            })
            .RequireAuthorization()
            .WithTags(Tags.Categories)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<CategoryDto>));
        }
    }
}
