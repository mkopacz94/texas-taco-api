using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class AccountDeletedEventMessageConsumer(
        IInboxMessagesRepository<InboxMessage<AccountDeletedEventMessage>> inboxRepository,
        ILogger<AccountDeletedEventMessageConsumer> logger)
        : IConsumer<AccountDeletedEventMessage>
    {
        private readonly IInboxMessagesRepository<InboxMessage<AccountDeletedEventMessage>>
            _inboxRepository = inboxRepository;
        private readonly ILogger<AccountDeletedEventMessageConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<AccountDeletedEventMessage> context)
        {
            var message = context.Message;
            var inboxMessage = new InboxMessage<AccountDeletedEventMessage>(message);

            try
            {
                if (await _inboxRepository.ContainsMessageWithSameId(message.Id))
                {
                    _logger.LogInformation("Inbox already contains {messageType} " +
                        "with id {id}. Message ignored.",
                        nameof(InboxMessage<AccountDeletedEventMessage>),
                        message.Id.ToString());

                    return;
                }

                await _inboxRepository.AddAsync(inboxMessage);

                _logger.LogInformation("Consumed {messageType} with id {id} and " +
                    "successfully added it to the inbox.",
                    nameof(InboxMessage<AccountDeletedEventMessage>),
                    message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume {message type} with id {id}.",
                    nameof(InboxMessage<AccountDeletedEventMessage>),
                    message.Id.ToString());

                throw;
            }
        }
    }
}
