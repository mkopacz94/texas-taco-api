using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IEmailNotificationsRepository
    {
        Task AddAsync(EmailNotification notification);
        Task<IEnumerable<EmailNotification>> GetPendingAsync();
        Task UpdateAsync(EmailNotification notification);
    }
}