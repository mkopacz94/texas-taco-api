using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Repositories;

namespace TexasTaco.Products.Core.Services
{
    internal class ProductPriceChangedOutboxMessagesProcessor(
        IUnitOfWork unitOfWork,
        IProductPriceChangedOutboxMessagesRepository outboxRepository,
        IBus messageBus,
        ILogger<ProductPriceChangedOutboxMessagesProcessor> logger)
        : IProductPriceChangedOutboxMessagesProcessor
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IProductPriceChangedOutboxMessagesRepository _outboxRepository
            = outboxRepository;
        private readonly IBus _messageBus = messageBus;
        private readonly ILogger<ProductPriceChangedOutboxMessagesProcessor> _logger = logger;

        public async Task ProcessMessages()
        {
            var messagesToBePublished = await _outboxRepository
                .GetNonPublishedOutboxMessages();

            foreach (var message in messagesToBePublished)
            {
                try
                {
                    _logger.LogInformation("Processing product price changed " +
                        "outbox message with Id={messageId}...", message.Id);

                    using var transaction = await _unitOfWork.BeginTransactionAsync();

                    message.MarkAsPublished();
                    await _outboxRepository.UpdateAsync(message);

                    await _messageBus.Publish(message.MessageBody);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Published product price changed message. " +
                            "Product Id: {id}, New price: {newPrice}.",
                        message.MessageBody.ProductId,
                        message.MessageBody.NewPrice);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occured during processing product price changed " +
                            "outbox message with Id={messageId}.",
                        message.Id);
                }
            }
        }
    }
}
