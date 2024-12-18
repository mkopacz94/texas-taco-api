using Microsoft.Extensions.Logging;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Core.Services.Inbox
{
    internal class PointsCollectedInboxMessagesProcessor(
        IUnitOfWork unitOfWork,
        IPointsCollectedInboxMessagesRepository inboxRepository,
        IUsersRepository usersRepository,
        ILogger<PointsCollectedInboxMessagesProcessor> logger)
        : IPointsCollectedInboxMessagesProcessor
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPointsCollectedInboxMessagesRepository _inboxRepository
            = inboxRepository;
        private readonly IUsersRepository _usersRepository = usersRepository;
        private readonly ILogger<PointsCollectedInboxMessagesProcessor> _logger
            = logger;

        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing points collected inbox " +
                    "message with Id={messageId}...", message.Id.Value);

                try
                {
                    using var transaction = await _unitOfWork.BeginTransactionAsync();

                    var accountId = message
                        .MessageBody
                        .AccountId;

                    var userWithSameAccount = await _usersRepository
                        .GetByAccountIdAsync(accountId);

                    if (userWithSameAccount is null)
                    {
                        _logger.LogError(
                            "User with accountId={accountId} does not " +
                            "exist and points collected cannot be updated.",
                            accountId);

                        continue;
                    }

                    userWithSameAccount.AddPoints(
                        message.MessageBody.PointsCollected);

                    await _usersRepository
                        .UpdateUserAsync(userWithSameAccount);

                    message.MarkAsProcessed();
                    await _inboxRepository.UpdateAsync(message);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Successfully processed " +
                        "message with Id={messageId}.", message.Id.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occured during processing inbox message with Id={messageId}.",
                        message.Id.Value);
                }
            }
        }
    }
}
