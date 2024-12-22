using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.PointsCollectedOutbox;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;

namespace TexasTaco.Orders.Infrastructure.Outbox
{
    internal class PointsCollectedOutboxMessagesProcessor(
        IUnitOfWork unitOfWork,
        IOutboxMessagesRepository<OutboxMessage<PointsCollectedEventMessage>>
            outboxRepository,
        IBus messageBus,
        ILogger<PointsCollectedOutboxMessagesProcessor> logger)
        : IPointsCollectedOutboxMessagesProcessor
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutboxMessagesRepository<OutboxMessage<PointsCollectedEventMessage>>
            _outboxRepository = outboxRepository;
        private readonly IBus _messageBus = messageBus;
        private readonly ILogger<PointsCollectedOutboxMessagesProcessor> _logger = logger;

        public async Task ProcessMessages()
        {
            var messagesToBePublished = await _outboxRepository
                .GetNonPublishedMessages();

            foreach (var message in messagesToBePublished)
            {
                _logger.LogInformation("Processing points collected " +
                    "outbox message with Id={messageId}.", message.Id);

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
                        message.Id);
                }
            }
        }
    }
}
