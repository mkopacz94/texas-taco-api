using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class VerificationTokensRepository(
        AuthDbContext _authDbContext,
        ILogger<VerificationTokensRepository> _logger) : IVerificationTokensRepository
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

        public async Task DeleteTokensExpiredEarlierThan(TimeSpan expiredTimeSpan)
        {
            var verificationTokens = await _authDbContext
                .VerificationTokens
                .ToListAsync();

            var tokensToBeDeleted = verificationTokens
                .Where(t => t.ExpiredEarlierThan(expiredTimeSpan))
                .ToList();

            _logger.LogInformation("{amount} verification " +
                "tokens expired {days} days ago will be deleted.", 
                tokensToBeDeleted.Count, expiredTimeSpan.Days);

            _authDbContext.RemoveRange(tokensToBeDeleted);
            await _authDbContext.SaveChangesAsync();
        }
    }
}
