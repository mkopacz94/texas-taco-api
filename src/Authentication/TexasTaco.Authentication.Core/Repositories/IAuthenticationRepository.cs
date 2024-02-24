using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Account> CreateAccountAsync(EmailAddress email, Role role, string password);
        Task<Account> AuthenticateAccountAsync(EmailAddress email, string password);
        Task<Account?> GetByIdAsync(AccountId id);
        Task UpdateAccountAndAddAccountCreatedOutboxMessage(
            Account account, AccountCreatedOutbox accountCreatedOutbox);
        Task<IEnumerable<AccountCreatedOutbox>> GetNonPublishedUserCreatedOutboxMessages();
    }
}
