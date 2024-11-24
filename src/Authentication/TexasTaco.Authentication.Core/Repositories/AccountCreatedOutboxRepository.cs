using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AccountCreatedOutboxRepository(AuthDbContext _dbContext)
        : IAccountCreatedOutboxRepository
    {
        public async Task AddAsync(AccountCreatedOutbox accountCreatedOutboxMessage)
        {
            await _dbContext.AddAsync(accountCreatedOutboxMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountCreatedOutbox accountCreatedOutboxMessage)
        {
            _dbContext.AccountsCreatedOutbox.Update(accountCreatedOutboxMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountCreatedOutbox>> GetNonPublishedAccountCreatedOutboxMessages()
        {
            return await _dbContext.AccountsCreatedOutbox
                .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }
    }
}
