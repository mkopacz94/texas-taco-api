using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Mapping;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Pagination;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    internal class GetPrizes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("prizes", async (
                IPrizesRepository prizesRepository,
                [FromQuery] int? pageNumber,
                [FromQuery] int? pageSize,
                [FromQuery] string? searchQuery) =>
            {
                if (pageNumber is null && pageSize is null)
                {
                    var prizes = await prizesRepository.GetAllAsync();
                    var allPrizesDtos = prizes
                        .Select(p => PrizeMap.Map(p))
                        .ToList();

                    return Results.Ok(allPrizesDtos);
                }

                if (pageNumber <= 0)
                {
                    return Results.BadRequest("Page number must be a positive number.");
                }

                if (pageSize <= 0)
                {
                    return Results.BadRequest("Page size must be a positive number.");
                }

                var pagedPrizes = await prizesRepository
                    .GetPagedPrizesAsync(
                        (int)pageNumber!,
                        (int)pageSize!,
                        searchQuery);

                var prizesDtos = pagedPrizes
                    .Items
                    .Select(p => PrizeMap.Map(p))
                    .ToList();

                var pagedResultDto = new PagedResult<PrizeDto>(
                    prizesDtos,
                    pagedPrizes.TotalCount,
                    pagedPrizes.PageSize,
                    pagedPrizes.CurrentPage);

                return Results.Ok(pagedResultDto);
            })
            .RequireAuthorization()
            .WithTags(Tags.Prizes)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<PrizeDto>))
            .Produces(StatusCodes.Status200OK, typeof(PagedResult<PrizeDto>))
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
