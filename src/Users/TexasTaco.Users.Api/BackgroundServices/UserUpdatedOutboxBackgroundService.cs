using TexasTaco.Users.Core.Services.Outbox;

namespace TexasTaco.Users.Api.BackgroundServices
{
    public class UserUpdatedOutboxBackgroundService(
        IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var messagesProcessor = scope
                    .ServiceProvider
                    .GetRequiredService<IUserUpdatedOutboxMessagesProcessor>();

                await messagesProcessor.ProcessMessages();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
