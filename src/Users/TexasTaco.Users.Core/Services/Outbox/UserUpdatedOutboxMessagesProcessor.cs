using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;
using TexasTaco.Users.Core.Data.EF;

namespace TexasTaco.Users.Core.Services.Outbox
{
    internal class UserUpdatedOutboxMessagesProcessor(
        IUnitOfWork _unitOfWork,
        IOutboxMessagesRepository<OutboxMessage<UserUpdatedEventMessage>> _outboxRepository,
        IBus _messageBus,
        ILogger<UserUpdatedOutboxMessagesProcessor> _logger)
        : IUserUpdatedOutboxMessagesProcessor
    {
        public async Task ProcessMessages()
        {
            var messagesToBePublished = await _outboxRepository
                .GetNonPublishedMessages();

            foreach (var message in messagesToBePublished)
            {
                try
                {
                    _logger.LogInformation("Processing user updated outbox message with Id={messageId}...", message.Id);

                    using var transaction = await _unitOfWork.BeginTransactionAsync();

                    message.MarkAsPublished();
                    await _outboxRepository.UpdateAsync(message);

                    await _messageBus.Publish(message.MessageBody);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Published user updated message. " +
                            "Id: {id}, AccountId: {accountId}, FirstName: {firstName}, " +
                            "LastName: {lastName}, AddressLine: {addressLine}, " +
                            "PostalCode: {postalCode}, City: {city}, " +
                            "Country: {country}",
                        message.MessageBody.Id,
                        message.MessageBody.AccountId,
                        message.MessageBody.FirstName,
                        message.MessageBody.LastName,
                        message.MessageBody.AddressLine,
                        message.MessageBody.PostalCode,
                        message.MessageBody.City,
                        message.MessageBody.Country);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occured during processing user updated " +
                            "outbox message with Id={messageId}.",
                        message.Id);
                }
            }
        }
    }
}
