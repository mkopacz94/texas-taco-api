using Microsoft.EntityFrameworkCore;
using TexasTaco.Shared.Outbox;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    internal class UserUpdatedOutboxMessagesRepository(UsersDbContext _dbContext)
        : IUserUpdatedOutboxMessagesRepository
    {
        public async Task AddAsync(UserUpdatedOutboxMessage outboxMessage)
        {
            await _dbContext.AddAsync(outboxMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserUpdatedOutboxMessage>> GetNonPublishedOutboxMessages()
        {
            return await _dbContext.UserUpdatedOutboxMessages
                .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }

        public async Task UpdateAsync(UserUpdatedOutboxMessage outboxMessage)
        {
            _dbContext.UserUpdatedOutboxMessages.Update(outboxMessage);
            await _dbContext.SaveChangesAsync();
        }
    }
}
