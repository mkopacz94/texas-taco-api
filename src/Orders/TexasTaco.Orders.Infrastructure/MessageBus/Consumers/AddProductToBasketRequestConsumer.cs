using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Shared.EventBus.Products;

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

            var basketItem = new BasketItem(
                busMessage.ProductId,
                busMessage.Name,
                busMessage.Price,
                busMessage.PictureUrl,
                busMessage.Quantity);

            var basket = await _basketService
                .AddItemToBasket(busMessage.AccountId, basketItem);

            string productLocation = $"/api/v1/orders/basket/{basket.Id.Value}/items/{basketItem.Id.Value}";
            var response = new AddProductToBasketResponse(true, productLocation);
            await context.RespondAsync(response);
        }
    }
}
