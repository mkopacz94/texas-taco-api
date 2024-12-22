using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class PointsCollectedEventMessageConsumer(
        IInboxMessagesRepository<InboxMessage<PointsCollectedEventMessage>> _inboxRepository,
        ILogger<PointsCollectedEventMessageConsumer> _logger)
        : IConsumer<PointsCollectedEventMessage>
    {
        public async Task Consume(ConsumeContext<PointsCollectedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new InboxMessage<PointsCollectedEventMessage>(message);

            try
            {
                if (await _inboxRepository.ContainsMessageWithSameId(message.Id))
                {
                    _logger.LogInformation("Inbox already contains {messageType} " +
                        "with id {id}. Message ignored.",
                        nameof(InboxMessage<PointsCollectedEventMessage>),
                        message.Id.ToString());

                    return;
                }

                await _inboxRepository.AddAsync(inboxMessage);

                _logger.LogInformation("Consumed {messageType} with id {id} and " +
                    "successfully added it to the inbox.",
                    nameof(InboxMessage<PointsCollectedEventMessage>),
                    message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume {message type} with id {id}.",
                    nameof(InboxMessage<PointsCollectedEventMessage>),
                    message.Id.ToString());

                throw;
            }
        }
    }
}
