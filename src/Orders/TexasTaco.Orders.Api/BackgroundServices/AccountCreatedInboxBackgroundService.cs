using TexasTaco.Orders.Application.AccountCreatedInbox;

namespace TexasTaco.Orders.Api.BackgroundServices
{
    public class AccountCreatedInboxBackgroundService(
        IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var messagesProcessor = scope
                    .ServiceProvider
                    .GetRequiredService<IAccountCreatedInboxMessagesProcessor>();

                await messagesProcessor.ProcessMessages();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
