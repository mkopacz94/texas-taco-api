
using TexasTaco.Authentication.Core.Repositories;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    internal class DatabaseCleaningBackgroundService(
        ILogger<DatabaseCleaningBackgroundService> _logger,
        IServiceProvider _serviceProvider) 
        : BackgroundService
    {
        private const int DaysSinceExpirationIndicatingThatTokenShouldBeDeleted = 30;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await CleanVerificationTokensTable();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task CleanVerificationTokensTable()
        {
            using var scope = _serviceProvider.CreateScope();

            var verificationTokensRepository = scope
                .ServiceProvider
                .GetRequiredService<IVerificationTokensRepository>();

            _logger.LogInformation(
                "Removing tokens expired {days} days ago.",
                DaysSinceExpirationIndicatingThatTokenShouldBeDeleted);

            await verificationTokensRepository
                .DeleteTokensExpiredEarlierThan(
                    TimeSpan.FromDays(DaysSinceExpirationIndicatingThatTokenShouldBeDeleted));
        }
    }
}
