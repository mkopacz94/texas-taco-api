using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.PointsCollectedOutbox;
using TexasTaco.Orders.Application.Shared;

namespace TexasTaco.Orders.Infrastructure.Outbox
{
    internal class PointsCollectedOutboxMessagesProcessor(
        IUnitOfWork unitOfWork,
        IPointsCollectedOutboxMessagesRepository outboxRepository,
        IBus messageBus,
        ILogger<PointsCollectedOutboxMessagesProcessor> logger)
        : IPointsCollectedOutboxMessagesProcessor
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPointsCollectedOutboxMessagesRepository _outboxRepository
            = outboxRepository;
        private readonly IBus _messageBus = messageBus;
        private readonly ILogger<PointsCollectedOutboxMessagesProcessor> _logger = logger;

        public async Task ProcessMessages()
        {
            var messagesToBePublished = await _outboxRepository
                .GetNonPublishedOutboxMessages();

            foreach (var message in messagesToBePublished)
            {
                _logger.LogInformation("Processing points collected " +
                    "outbox message with Id={messageId}.", message.Id.Value);

                try
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () =>
                    {
                        message.MarkAsPublished();
                        await _outboxRepository.UpdateAsync(message);

                        await _messageBus.Publish(message.MessageBody);

                        _logger.LogInformation("Published points collected message. " +
                                "Account Id: {id}, Points collected: {pointsCollected}.",
                            message.MessageBody.AccountId,
                            message.MessageBody.PointsCollected);
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occured during processing outbox message with Id={messageId}.",
                        message.Id.Value);
                }
            }
        }
    }
}
