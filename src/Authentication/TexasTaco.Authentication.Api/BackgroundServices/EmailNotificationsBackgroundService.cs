using TexasTaco.Authentication.Core.Repositories;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    public class EmailNotificationsBackgroundService(
        IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var emailNotificationsRepository = scope.ServiceProvider
                    .GetRequiredService<IEmailNotificationsRepository>();

                var emails = await emailNotificationsRepository.GetPendingAsync();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
