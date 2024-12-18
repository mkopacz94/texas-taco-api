﻿using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers.CreateCustomer;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.AccountCreatedInbox
{
    internal class AccountCreatedInboxMessagesProcessor(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IAccountCreatedInboxMessagesRepository inboxRepository,
        ILogger<AccountCreatedInboxMessagesProcessor> logger)
        : IAccountCreatedInboxMessagesProcessor
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMediator _mediator = mediator;
        private readonly IAccountCreatedInboxMessagesRepository _inboxRepository = inboxRepository;
        private readonly ILogger<AccountCreatedInboxMessagesProcessor> _logger = logger;

        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedAccountCreatedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing account created " +
                    "inbox message with Id={messageId}...", message.Id.Value);

                try
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        var createCustomerCommand = new CreateCustomerCommand(
                            new AccountId(message.MessageBody.AccountId),
                            message.MessageBody.Email);

                        await _mediator.Send(createCustomerCommand);
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
                        "Error occured during processing inbox message " +
                            "with Id={messageId}.",
                        message.Id.Value);
                }
            }
        }
    }
}
