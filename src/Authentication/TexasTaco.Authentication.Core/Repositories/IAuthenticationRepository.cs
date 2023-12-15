using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Account> CreateAccountAsync(EmailAddress email, Role role, string password);
        Task<Account> AuthenticateAccountAsync(EmailAddress email, string password);
        Task<Account?> GetByIdAsync(AccountId id);
        Task UpdateAccountAsync(Account account);
    }
}
