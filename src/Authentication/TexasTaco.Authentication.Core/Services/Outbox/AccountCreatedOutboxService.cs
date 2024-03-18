using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    internal class AccountCreatedOutboxService(
        AuthDbContext _dbContext, IBus _messageBus, ILogger<AccountCreatedOutboxService> _logger) 
        : IAccountCreatedOutboxService
    {
        public async Task PublishAccountCreatedOutboxMessage(AccountCreatedOutbox message)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            message.MarkAsPublished();
            _dbContext.Update(message);

            await _messageBus.Publish(new AccountCreatedEventMessage(
                Guid.NewGuid(),
                message.AccountId.Value,
                message.UserEmail,
                DateTime.UtcNow));

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Published account created message. " +
                "AccountId: {accountId}, Email address: {emailAddress}", 
                message.AccountId.Value, message.UserEmail.Value);
        }
    }
}
