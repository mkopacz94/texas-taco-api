using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.AccountCreatedInbox;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Application.Customers.UpdateCustomer;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.UserUpdatedInbox
{
    internal class UserUpdatedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IMediator _mediator,
        ICustomersRepository _customersRepository,
        IUserUpdatedInboxMessagesRepository _inboxRepository,
        ILogger<AccountCreatedInboxMessagesProcessor> _logger)
        : IUserUpdatedInboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing user updated " +
                    "inbox message with Id={messageId}...", message.Id.Value);

                try
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        var accountId = new AccountId(message.MessageBody.AccountId);

                        var customerToUpdate = await _customersRepository
                            .GetByAccountIdAsync(accountId)
                            ?? throw new CustomerWithAccountIdNotFoundException(accountId);

                        var address = new Address(
                            message.MessageBody.AddressLine,
                            message.MessageBody.PostalCode,
                            message.MessageBody.City,
                            message.MessageBody.Country);

                        var updateCustomerCommand = new UpdateCustomerCommand(
                            customerToUpdate.Id,
                            message.MessageBody.FirstName,
                            message.MessageBody.LastName,
                            address);

                        await _mediator.Send(updateCustomerCommand);

                        message.MarkAsProcessed();

                        await _inboxRepository.UpdateAsync(message);

                        _logger.LogInformation("Successfully processed " +
                            "message with Id={messageId}.", message.Id.Value);
                    });
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
