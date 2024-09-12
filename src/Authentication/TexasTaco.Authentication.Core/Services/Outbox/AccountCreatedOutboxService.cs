using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Authentication.Core.Data;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    internal class AccountCreatedOutboxService(
        IUnitOfWork _unitOfWork, 
        IAccountCreatedOutboxRepository _accountCreatedOutboxRepository,
        IBus _messageBus, 
        ILogger<AccountCreatedOutboxService> _logger) 
        : IAccountCreatedOutboxService
    {
        public async Task PublishAccountCreatedOutboxMessage(AccountCreatedOutbox message)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            message.MarkAsPublished();
            await _accountCreatedOutboxRepository.UpdateAsync(message);

            await _messageBus.Publish(new AccountCreatedEventMessage(
                Guid.NewGuid(),
                message.AccountId.Value,
                message.UserEmail,
                DateTime.UtcNow));

            await transaction.CommitAsync();

            _logger.LogInformation("Published account created message. " +
                "AccountId: {accountId}, Email address: {emailAddress}", 
                message.AccountId.Value, message.UserEmail.Value);
        }
    }
}
