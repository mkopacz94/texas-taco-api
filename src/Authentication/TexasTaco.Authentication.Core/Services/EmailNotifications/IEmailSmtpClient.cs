using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Services.EmailNotifications
{
    public interface IEmailSmtpClient
    {
        Task SendAsync(EmailNotification notificationData);
    }
}
