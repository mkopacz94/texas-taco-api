using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers.CreateCustomer;
using TexasTaco.Orders.Application.UnitOfWork;

namespace TexasTaco.Orders.Application.AccountCreatedInbox
{
    internal class AccountCreatedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IMediator _mediator,
        IAccountCreatedInboxMessagesRepository _inboxRepository,
        ILogger<AccountCreatedInboxMessagesProcessor> _logger)
        : IAccountCreatedInboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedAccountCreatedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing account created " +
                    "inbox message with Id={messageId}...", message.Id);

                try
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        var createCustomerCommand = new CreateCustomerCommand(
                            message.MessageBody.AccountId,
                            message.MessageBody.Email);

                        await _mediator.Send(createCustomerCommand);
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
