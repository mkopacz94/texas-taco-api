using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Services.EmailNotifications
{
    public interface IEmailSmtpClient
    {
        Task SendAsync(EmailNotification notificationData);
    }
}
