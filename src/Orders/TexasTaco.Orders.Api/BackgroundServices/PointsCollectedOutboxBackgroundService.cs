using TexasTaco.Orders.Application.PointsCollectedOutbox;

namespace TexasTaco.Orders.Api.BackgroundServices
{
    public class PointsCollectedOutboxBackgroundService(
        IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var messagesProcessor = scope
                    .ServiceProvider
                    .GetRequiredService<IPointsCollectedOutboxMessagesProcessor>();

                await messagesProcessor.ProcessMessages();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
