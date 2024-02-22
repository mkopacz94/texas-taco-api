using MassTransit;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    public class UsersCreatedOutboxBackgroundService(
        ILogger<UsersCreatedOutboxBackgroundService> _logger,
        IServiceProvider _serviceProvider)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var usersCreatedOutboxRepository = scope
                    .ServiceProvider
                    .GetRequiredService<IUsersCreatedOutboxRepository>();

                var messagesToBePublished = await usersCreatedOutboxRepository
                    .GetOutboxMessagesToBePublishedAsync();

                var messageBus = scope
                    .ServiceProvider
                    .GetRequiredService<IBus>();

                foreach (var message in messagesToBePublished)
                {
                    try
                    {
                        _logger.LogInformation("Processing outbox message with Id={messageId}...", message.Id);

                        message.MarkAsPublished();

                        await usersCreatedOutboxRepository
                            .UpdateInTransactionAsync(message, async () =>
                        {
                            await messageBus.Publish(new AccountCreatedEventMessage(
                                Guid.NewGuid(),
                                message.UserEmail,
                                DateTime.UtcNow));
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
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
