using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class VerificationTokensRepository(AuthDbContext _authDbContext) : IVerificationTokensRepository
    {
        public async Task AddAsync(VerificationToken token)
        {
            await _authDbContext.AddAsync(token);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<VerificationToken?> GetByTokenValueAsync(Guid tokenValue)
        {
            return await _authDbContext.VerificationTokens
                .FirstOrDefaultAsync(t => t.Token == tokenValue);
        }
    }
}
