
using Asp.Versioning;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Products.Api.Endpoints.Prizes
{
    public class DeletePrize : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("prizes/{id}", [AuthorizeRole(Role.Admin)] async (
                string id,
                IPrizesRepository prizesRepository) =>
            {
                if (!Guid.TryParse(id, out Guid idGuid))
                {
                    throw new InvalidRequestParametersException(
                        $"Given prize ID ({id}) is not a valid GUID.");
                }

                var prizeId = new PrizeId(idGuid);
                await prizesRepository.DeleteAsync(prizeId);

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
