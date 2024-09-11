using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;

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
    }
}
