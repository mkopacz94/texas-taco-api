using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IVerificationTokensRepository
    {
        Task AddAsync(VerificationToken token);
        Task<VerificationToken?> GetByTokenValueAsync(Guid tokenValue);
        Task DeleteTokensExpiredEarlierThan(TimeSpan expiredTimeSpan);
    }
}
