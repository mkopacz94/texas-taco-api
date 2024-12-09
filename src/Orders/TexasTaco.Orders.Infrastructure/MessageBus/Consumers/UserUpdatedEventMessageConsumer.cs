using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.UserUpdatedInbox;
using TexasTaco.Orders.Persistence.UserUpdatedInboxMessages;
using TexasTaco.Shared.EventBus.Users;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class UserUpdatedEventMessageConsumer(
        IUserUpdatedInboxMessagesRepository _inboxRepository,
        ILogger<UserUpdatedEventMessageConsumer> _logger)
        : IConsumer<UserUpdatedEventMessage>
    {
        public async Task Consume(ConsumeContext<UserUpdatedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new UserUpdatedInboxMessage(message);

            try
            {
                if (await _inboxRepository.ContainsMessageWithSameId(message.Id))
                {
                    _logger.LogInformation("Inbox already contains {messageType} " +
                        "with id {id}. Message ignored.",
                        nameof(UserUpdatedEventMessage),
                        message.Id.ToString());

                    return;
                }

                await _inboxRepository.AddAsync(inboxMessage);

                _logger.LogInformation("Consumed {messageType} with id {id} and " +
                    "successfully added it to the inbox.",
                    nameof(UserUpdatedEventMessage),
                    message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume {message type} with id {id}.",
                    nameof(UserUpdatedEventMessage),
                    message.Id.ToString());

                throw;
            }
        }
    }
}
