using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Authentication;
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
                [FromServices] ILogger<AddToBasket> logger,
                ClaimsPrincipal user) =>
            {
                var productId = new ProductId(Guid.Parse(id));
                var productToAdd = await productsRepository.GetAsync(productId);

                if (productToAdd is null)
                {
                    return Results.BadRequest($"Product with id {productId.Value} not found.");
                }

                string currentUserAccountId = user.FindFirst(TexasTacoClaimNames.AccountId)!.Value;

                var request = new AddProductToBasketRequest(
                    Guid.Parse(currentUserAccountId),
                    productToAdd.Id,
                    productToAdd.Name,
                    productToAdd.Price,
                    productToAdd.Picture?.Url,
                    quantity);

                var response = await busClient.GetResponse<AddProductToBasketResponse>(request);
                var message = response.Message;

                logger.LogInformation(
                    "Received AddProductToBasketResponse. {jsonObject}",
                    JsonSerializer.Serialize(message));

                if (!message.IsSuccess)
                {
                    return Results.BadRequest(message.ErrorMessage);
                }

                return Results.Created(response.Message.ProductLocation, null);
            })
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(Product));
        }
    }
}
