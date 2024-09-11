using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAccountCreatedOutboxRepository
    {
        Task AddAsync(AccountCreatedOutbox accountCreatedOutboxMessage);
    }
}
