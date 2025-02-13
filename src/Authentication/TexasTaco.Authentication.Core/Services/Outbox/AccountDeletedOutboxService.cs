using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Authentication.Core.Data;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    internal class AccountDeletedOutboxService(
        IUnitOfWork unitOfWork,
        IOutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>
            outboxRepository,
        IBus messageBus,
        ILogger<AccountDeletedOutboxService> logger)
        : IAccountDeletedOutboxService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>
            _outboxRepository = outboxRepository;
        private readonly IBus _messageBus = messageBus;
        private readonly ILogger<AccountDeletedOutboxService> _logger = logger;

        public async Task PublishOutboxMessage(OutboxMessage<AccountDeletedEventMessage> message)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            message.MarkAsPublished();
            await _outboxRepository.UpdateAsync(message);

            await _messageBus.Publish(new AccountDeletedEventMessage(
                Guid.NewGuid(),
                message.MessageBody.AccountId));

            await transaction.CommitAsync();

            _logger.LogInformation("Published account deleted message. " +
                "AccountId: {accountId}.",
                message.MessageBody.AccountId);
        }
    }
}
