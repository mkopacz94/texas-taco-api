using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class ProductPriceChangedEventMessageConsumer(
        IInboxMessagesRepository<InboxMessage<ProductPriceChangedEventMessage>> _inboxRepository,
        ILogger<ProductPriceChangedEventMessageConsumer> _logger)
        : IConsumer<ProductPriceChangedEventMessage>
    {
        public async Task Consume(ConsumeContext<ProductPriceChangedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new InboxMessage<ProductPriceChangedEventMessage>(message);

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
