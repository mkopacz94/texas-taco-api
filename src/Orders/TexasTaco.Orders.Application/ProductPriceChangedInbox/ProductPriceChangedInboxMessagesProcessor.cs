﻿using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Carts.UpdateProductPrice;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

namespace TexasTaco.Orders.Application.ProductPriceChangedInbox
{
    internal class ProductPriceChangedInboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IMediator _mediator,
        IInboxMessagesRepository<InboxMessage<ProductPriceChangedEventMessage>> _inboxRepository,
        ILogger<ProductPriceChangedInboxMessagesProcessor> _logger)
        : IProductPriceChangedInboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var nonProcessedMessages = await _inboxRepository
                .GetNonProcessedMessages();

            foreach (var message in nonProcessedMessages)
            {
                _logger.LogInformation("Processing product price changed " +
                    "inbox message with Id={messageId}...", message.Id);

                try
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        var updateProductPriceCommand = new UpdateProductPriceCommand(
                            message.MessageBody.ProductId,
                            message.MessageBody.NewPrice);

                        await _mediator.Send(updateProductPriceCommand);

                        message.MarkAsProcessed();

                        await _inboxRepository.UpdateAsync(message);

                        _logger.LogInformation("Successfully processed " +
                            "message with Id={messageId}.", message.Id);
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
