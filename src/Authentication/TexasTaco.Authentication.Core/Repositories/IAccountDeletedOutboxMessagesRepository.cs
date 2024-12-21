using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAccountDeletedOutboxMessagesRepository
    {
        Task AddAsync(AccountDeletedOutboxMessage message);
        Task UpdateAsync(AccountDeletedOutboxMessage message);
        Task<IEnumerable<AccountDeletedOutboxMessage>> GetNonPublishedMessages();
    }
}
