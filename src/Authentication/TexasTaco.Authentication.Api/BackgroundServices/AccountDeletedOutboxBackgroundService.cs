using TexasTaco.Authentication.Core.Services.Outbox;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    public class AccountDeletedOutboxBackgroundService(
        ILogger<AccountDeletedOutboxBackgroundService> _logger,
        IServiceProvider _serviceProvider)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var outboxRepository = scope
                    .ServiceProvider
                    .GetRequiredService<IOutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>>();

                var outboxService = scope
                    .ServiceProvider
                    .GetRequiredService<IAccountDeletedOutboxService>();

                var messagesToBePublished = await outboxRepository
                    .GetNonPublishedMessages();

                foreach (var message in messagesToBePublished)
                {
                    try
                    {
                        _logger.LogInformation("Processing outbox message with Id={messageId}...", message.Id);

                        await outboxService.PublishOutboxMessage(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error occured during processing outbox message with Id={messageId}.",
                            message.Id);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
