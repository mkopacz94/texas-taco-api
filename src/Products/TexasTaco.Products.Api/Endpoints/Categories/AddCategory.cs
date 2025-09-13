
using Asp.Versioning;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;

namespace TexasTaco.Products.Api.Endpoints.Categories
{
    internal class AddCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("categories", [AuthorizeRole(Role.Admin)] async (
                AddCategoryDto categoryDto,
                ICategoriesRepository categoriesRepository) =>
            {
                string trimmedName = categoryDto.Name.Trim();

                string capitalizedCategory = string.IsNullOrEmpty(trimmedName)
                    ? trimmedName
                    : char.ToUpper(trimmedName[0]) + trimmedName.Substring(1);

                var category = new Category(categoryDto.Name);

                await categoriesRepository.AddAsync(category);

                return Results.CreatedAtRoute(
                    Routes.GetCategory,
                    new { Id = category.Id.Value });
            })
            .RequireAuthorization()
            .WithTags(Tags.Categories)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Category))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}
