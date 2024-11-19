using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class AddToBasket : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("basket-add/{id}", async (
                string id,
                [FromQuery] int quantity,
                [FromServices] IProductsRepository productsRepository,
                [FromServices] IRequestClient<AddProductToBasketRequest> busClient,
                [FromServices] ILogger<AddToBasket> logger) =>
            {
                var productId = new ProductId(Guid.Parse(id));
                var productToAdd = await productsRepository.GetAsync(productId);

                if (productToAdd is null)
                {
                    return Results.BadRequest($"Product with id {productId.Value} not found.");
                }

                var request = new AddProductToBasketRequest(
                    productToAdd.Id,
                    productToAdd.Name,
                    productToAdd.Price,
                    productToAdd.Picture?.Url,
                    quantity);

                var response = await busClient.GetResponse<AddProductToBasketResponse>(request);

                logger.LogInformation(
                    "Received AddProductToBasketResponse. {jsonObject}",
                    JsonSerializer.Serialize(response.Message));

                return Results.Created();
            })
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Product));
        }
    }
}
