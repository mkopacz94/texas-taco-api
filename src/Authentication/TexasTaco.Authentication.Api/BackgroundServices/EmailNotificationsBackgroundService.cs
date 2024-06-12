using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services.EmailNotifications;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    internal class EmailNotificationsBackgroundService(
        ILogger<EmailNotificationsBackgroundService> _logger,
        IServiceProvider _serviceProvider,
        IEmailSmtpClient _emailClient) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var emailNotificationsRepository = scope.ServiceProvider
                    .GetRequiredService<IEmailNotificationsRepository>();

                var emails = await emailNotificationsRepository.GetPendingAsync();

                foreach( var email in emails )
                {
                    try
                    {
                        await _emailClient.SendAsync(email);

                        email.MarkAsSent();
                        await emailNotificationsRepository.UpdateAsync(email);

                        _logger.LogInformation("Email notification to {email} has been sent.", email.To.Value);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Sending email notification " +
                            "with id {emailId} failed.", email.Id.Value);
                    }
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
