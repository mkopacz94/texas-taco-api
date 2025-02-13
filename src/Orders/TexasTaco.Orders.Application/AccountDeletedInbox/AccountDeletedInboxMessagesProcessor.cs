using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.AccountDeletedInbox
{
    internal class AccountDeletedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IInboxMessagesRepository<InboxMessage<AccountDeletedEventMessage>> _inboxRepository,
        ICustomersRepository _customersRepository,
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
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        var accountId = new AccountId(message.MessageBody.AccountId);

                        await _customersRepository
                            .DeleteByAccountIdAsync(accountId);

                        message.MarkAsProcessed();
                        await _inboxRepository.UpdateAsync(message);

                        _logger.LogInformation("Successfully processed " +
                            "message with Id={messageId}...", message.Id);
                    });
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
