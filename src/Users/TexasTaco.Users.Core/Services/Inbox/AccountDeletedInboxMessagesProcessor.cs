using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Core.Services.Inbox
{
    internal class AccountDeletedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IInboxMessagesRepository<InboxMessage<AccountDeletedEventMessage>> _inboxRepository,
        IUsersRepository _usersRepository,
        ILogger<AccountDeletedInboxMessagesProcessor> _logger)
        : IAccountDeletedInboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing account deleted inbox " +
                    "message with Id={messageId}...", message.Id);

                try
                {
                    using var transaction = await _unitOfWork.BeginTransactionAsync();

                    var accountId = new AccountId(message.MessageBody.AccountId);

                    await _usersRepository
                        .DeleteByAccountIdAsync(accountId);

                    message.MarkAsProcessed();
                    await _inboxRepository.UpdateAsync(message);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Successfully processed " +
                        "message with Id={messageId}...", message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occured during processing inbox message with Id={messageId}.",
                        message.Id);
                }
            }
        }
    }
}
