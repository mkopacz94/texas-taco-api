using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class VerificationTokensRepository(AuthDbContext _authDbContext) : IVerificationTokensRepository
    {
        public async Task AddAsync(VerificationToken token)
        {
            await _authDbContext.AddAsync(token);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<VerificationToken?> GetByAccoundId(AccountId accountId)
        {
            return await _authDbContext.VerificationTokens
                .FirstOrDefaultAsync(t => t.AccountId == accountId);
        }
    }
}
