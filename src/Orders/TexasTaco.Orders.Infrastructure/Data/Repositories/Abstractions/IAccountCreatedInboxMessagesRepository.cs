using TexasTaco.Orders.Domain.AccountCreatedInboxMessages;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories.Abstractions
{
    internal interface IAccountCreatedInboxMessagesRepository
    {
        Task AddAsync(AccountCreatedInboxMessage message);
        Task UpdateAsync(AccountCreatedInboxMessage message);
        Task<IEnumerable<AccountCreatedInboxMessage>> GetNonProcessedAccountCreatedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
