using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IVerificationTokensRepository
    {
        Task AddAsync(VerificationToken token);
        Task<VerificationToken?> GetByTokenValueAsync(Guid tokenValue);
    }
}
