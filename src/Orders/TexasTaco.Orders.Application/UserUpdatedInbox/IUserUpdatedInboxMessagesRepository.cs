using TexasTaco.Orders.Persistence.UserUpdatedInboxMessages;

namespace TexasTaco.Orders.Application.UserUpdatedInbox
{
    public interface IUserUpdatedInboxMessagesRepository
    {
        Task AddAsync(UserUpdatedInboxMessage message);
        Task UpdateAsync(UserUpdatedInboxMessage message);
        Task<IEnumerable<UserUpdatedInboxMessage>> GetNonProcessedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
