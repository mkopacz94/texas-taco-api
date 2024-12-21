
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    public class DeleteProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("{id}", async (
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
            });
        }
    }
}
