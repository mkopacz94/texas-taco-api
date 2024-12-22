using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class AccountCreatedEventMessageConsumer(
        IInboxMessagesRepository<InboxMessage<AccountCreatedEventMessage>> _inboxRepository,
        ILogger<AccountCreatedEventMessageConsumer> _logger)
        : IConsumer<AccountCreatedEventMessage>
    {
        public async Task Consume(ConsumeContext<AccountCreatedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new InboxMessage<AccountCreatedEventMessage>(message);

            try
            {
                if (await _inboxRepository.ContainsMessageWithSameId(message.Id))
                {
                    _logger.LogInformation("Inbox already contains {messageType} " +
                        "with id {id}. Message ignored.",
                        nameof(InboxMessage<AccountCreatedEventMessage>),
                        message.Id.ToString());

                    return;
                }

                await _inboxRepository.AddAsync(inboxMessage);

                _logger.LogInformation("Consumed {messageType} with id {id} and " +
                    "successfully added it to the inbox.",
                    nameof(InboxMessage<AccountCreatedEventMessage>),
                    message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume {message type} with id {id}.",
                    nameof(InboxMessage<AccountCreatedEventMessage>),
                    message.Id.ToString());

                throw;
            }
        }
    }
}
