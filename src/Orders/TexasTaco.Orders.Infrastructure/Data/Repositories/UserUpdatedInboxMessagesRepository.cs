using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.UserUpdatedInbox;
using TexasTaco.Orders.Domain.UserUpdatedInboxMessages;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class UserUpdatedInboxMessagesRepository(OrdersDbContext _dbContext)
        : IUserUpdatedInboxMessagesRepository
    {
        public async Task AddAsync(UserUpdatedInboxMessage message)
        {
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserUpdatedInboxMessage>> GetNonProcessedMessages()
        {
            return await _dbContext.UserUpdatedInboxMessages
                .Where(m => m.MessageStatus == InboxMessageStatus.ToBeProcessed)
                .ToListAsync();
        }

        public async Task UpdateAsync(UserUpdatedInboxMessage message)
        {
            _dbContext.Update(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ContainsMessageWithSameId(Guid id)
        {
            return await _dbContext
                .UserUpdatedInboxMessages
                .AnyAsync(m => m.MessageId.ToString() == id.ToString());
        }
    }
}
