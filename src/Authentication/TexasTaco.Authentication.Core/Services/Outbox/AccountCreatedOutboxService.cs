using MassTransit;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    internal class AccountCreatedOutboxService(AuthDbContext _dbContext, IBus _messageBus) 
        : IAccountCreatedOutboxService
    {
        public async Task PublishAccountCreatedOutboxMessage(AccountCreatedOutbox message)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            message.MarkAsPublished();
            _dbContext.Update(message);

            await _messageBus.Publish(new AccountCreatedEventMessage(
                Guid.NewGuid(),
                message.UserEmail,
                DateTime.UtcNow));

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}
