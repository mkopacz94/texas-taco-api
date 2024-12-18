using Microsoft.Extensions.Logging;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Core.Services.Inbox
{
    internal class AccountCreatedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IAccountCreatedInboxMessagesRepository _inboxRepository,
        IUsersRepository _usersRepository,
        ILogger<AccountCreatedInboxMessagesProcessor> _logger)
        : IAccountCreatedInboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedAccountCreatedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing account created inbox " +
                    "message with Id={messageId}...", message.Id);

                try
                {
                    using var transaction = await _unitOfWork.BeginTransactionAsync();

                    var user = new User(
                        message.MessageBody.AccountId,
                        message.MessageBody.Email);

                    var userWithSameAccount = await _usersRepository
                        .GetByAccountIdAsync(user.AccountId);

                    if (userWithSameAccount is not null)
                    {
                        _logger.LogError(
                            "User with accountId={accountId} already " +
                            "exists and cannot be added to database.",
                            user.AccountId);
                    }
                    else
                    {
                        await _usersRepository.AddUserAsync(user);
                    }

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
