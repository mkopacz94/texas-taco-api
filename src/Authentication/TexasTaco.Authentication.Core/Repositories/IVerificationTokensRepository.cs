using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IVerificationTokensRepository
    {
        Task AddAsync(VerificationToken token);
        Task<VerificationToken?> GetByAccoundId(AccountId accountId);
    }
}
