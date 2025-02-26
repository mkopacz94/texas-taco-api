using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Pagination;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class GetAllProducts : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("", async (
                IProductsRepository productsRepository,
                [FromQuery] int? pageNumber,
                [FromQuery] int? pageSize) =>
            {
                if (pageNumber is null && pageSize is null)
                {
                    var products = await productsRepository.GetAllAsync();

                    var response = products
                        .Select(p => ProductMap.Map(p))
                        .ToList();

                    return Results.Ok(response);
                }

                if (pageNumber <= 0)
                {
                    return Results.BadRequest("Page number must be a positive number.");
                }

                if (pageSize <= 0)
                {
                    return Results.BadRequest("Page size must be a positive number.");

                }

                var pagedProducts = await productsRepository
                    .GetPagedProductsAsync(
                        (int)pageNumber!,
                        (int)pageSize!,
                        null);

                var productsDtos = pagedProducts
                    .Items
                    .Select(p => ProductMap.Map(p))
                    .ToList();

                var pagedResultDto = new PagedResult<ProductDto>(
                    productsDtos,
                    pagedProducts.TotalCount,
                    pagedProducts.PageSize,
                    pagedProducts.CurrentPage);

                return Results.Ok(pagedResultDto);
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<ProductDto>))
            .Produces(StatusCodes.Status200OK, typeof(PagedResult<ProductDto>))
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
