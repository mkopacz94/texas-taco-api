using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TexasTaco.Shared.EventBus.Products;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class AddProductToBasketRequestConsumer(
        ILogger<AddProductToBasketRequestConsumer> _logger)
        : IConsumer<AddProductToBasketRequest>
    {
        public async Task Consume(ConsumeContext<AddProductToBasketRequest> context)
        {
            _logger.LogInformation(
                "Received AddProductToBasketRequest. {jsonObject}",
                JsonSerializer.Serialize(context.Message));

            var response = new AddProductToBasketResponse(true);
            await context.RespondAsync(response);
        }
    }
}
