using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class EmailNotificationsRepository(AuthDbContext _dbContext) : IEmailNotificationsRepository
    {
        public async Task AddAsync(EmailNotification notification)
        {
            await _dbContext.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmailNotification>> GetPendingAsync()
        {
            return await _dbContext.EmailNotifications
                .Where(n => n.Status == EmailNotificationStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(EmailNotification notification)
        {
            _dbContext.Update(notification);
            await _dbContext.SaveChangesAsync();
        }
    }
}
