using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class UsersCreatedOutboxRepository(AuthDbContext _dbContext) : IUsersCreatedOutboxRepository
    {
        public async Task AddAsync(UserCreatedOutbox userCreatedOutbox)
        {
            await _dbContext.AddAsync(userCreatedOutbox);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserCreatedOutbox>> GetOutboxMessagesToBePublishedAsync()
        {
            return await _dbContext.UsersCreatedOutbox
                .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }

        public async Task UpdateAsync(UserCreatedOutbox userCreatedOutbox)
        {
            _dbContext.Update(userCreatedOutbox);
            await _dbContext.SaveChangesAsync();
        }
    }
}
