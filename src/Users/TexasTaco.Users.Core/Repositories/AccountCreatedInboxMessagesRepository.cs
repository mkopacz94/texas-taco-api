using Microsoft.EntityFrameworkCore;
using TexasTaco.Shared.Inbox;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    internal class AccountCreatedInboxMessagesRepository(UsersDbContext _dbContext) 
        : IAccountCreatedInboxMessagesRepository
    {
        public async Task AddAsync(AccountCreatedInboxMessage message)
        {
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountCreatedInboxMessage>> GetNonProcessedAccountCreatedMessages()
        {
            return await _dbContext.AccountCreatedInboxMessages
                .Where(m => m.MessageStatus == InboxMessageStatus.ToBeProcessed)
                .ToListAsync();
        }

        public async Task UpdateAsync(AccountCreatedInboxMessage message)
        {
            _dbContext.Update(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ContainsMessageWithSameId(Guid id)
        {
            return await _dbContext
                .AccountCreatedInboxMessages
                .AnyAsync(m => m.MessageId.ToString() == id.ToString());
        }
    }
}
