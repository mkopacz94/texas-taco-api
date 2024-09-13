using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IAccountCreatedInboxMessagesRepository
    {
        Task AddAsync(AccountCreatedInboxMessage message);
        Task UpdateAsync(AccountCreatedInboxMessage message);
        Task<IEnumerable<AccountCreatedInboxMessage>> GetNonProcessedAccountCreatedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
