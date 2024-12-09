using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.ProductPriceChangedInbox;
using TexasTaco.Orders.Persistence.ProductPriceChangedInbox;
using TexasTaco.Shared.EventBus.Products;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class ProductPriceChangedEventMessageConsumer(
        IProductPriceChangedInboxMessagesRepository _inboxRepository,
        ILogger<ProductPriceChangedEventMessageConsumer> _logger)
        : IConsumer<ProductPriceChangedEventMessage>
    {
        public async Task Consume(ConsumeContext<ProductPriceChangedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new ProductPriceChangedInboxMessage(message);

            try
            {
                if (await _inboxRepository.ContainsMessageWithSameId(message.Id))
                {
                    _logger.LogInformation("Inbox already contains {messageType} " +
                        "with id {id}. Message ignored.",
                        nameof(ProductPriceChangedEventMessage),
                        message.Id.ToString());

                    return;
                }

                await _inboxRepository.AddAsync(inboxMessage);

                _logger.LogInformation("Consumed {messageType} with id {id} and " +
                    "successfully added it to the inbox.",
                    nameof(ProductPriceChangedEventMessage),
                    message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume {message type} with id {id}.",
                    nameof(ProductPriceChangedEventMessage),
                    message.Id.ToString());

                throw;
            }
        }
    }
}
