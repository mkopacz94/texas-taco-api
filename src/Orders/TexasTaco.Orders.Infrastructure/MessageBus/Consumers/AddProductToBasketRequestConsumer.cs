using Humanizer;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Basket.Exceptions;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class AddProductToBasketRequestConsumer(
        IBasketService _basketService,
        ILogger<AddProductToBasketRequestConsumer> _logger)
        : IConsumer<AddProductToBasketRequest>
    {
        public async Task Consume(ConsumeContext<AddProductToBasketRequest> context)
        {
            _logger.LogInformation(
                "Received AddProductToBasketRequest. {jsonObject}",
                JsonSerializer.Serialize(context.Message));

            var busMessage = context.Message;
            AddProductToBasketResponse response;

            try
            {
                var basketItem = new BasketItem(
                    busMessage.ProductId,
                    busMessage.Name,
                    busMessage.Price,
                    busMessage.PictureUrl,
                    busMessage.Quantity);

                var accountId = new AccountId(busMessage.AccountId);

                var basket = await _basketService
                    .AddItemToBasket(accountId, basketItem);

                string productLocation = $"/api/v1/orders/basket/{basket.Id.Value}/items/{basketItem.Id.Value}";
                response = new AddProductToBasketResponse(
                    true,
                    HttpStatusCode.Created,
                    productLocation);

                await context.RespondAsync(response);
            }
            catch (BasketItemException ex)
            {
                response = new AddProductToBasketResponse(
                    false,
                    ex.ExceptionCategory.AsStatusCode(),
                    ErrorMessage: BuildErrorMessage(ex));
            }

            await context.RespondAsync(response);
        }

        private static ErrorMessage BuildErrorMessage(Exception ex)
        {
            string errorCode = ex
                .GetType()
                .Name
                .Underscore()
                .Replace("_exception", string.Empty);

            return new ErrorMessage(
                errorCode,
                ex.Message);
        }
    }
}
