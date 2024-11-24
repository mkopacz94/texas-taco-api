using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IUserUpdatedOutboxMessagesRepository
    {
        Task AddAsync(UserUpdatedOutboxMessage outboxMessage);
        Task UpdateAsync(UserUpdatedOutboxMessage outboxMessage);
        Task<IEnumerable<UserUpdatedOutboxMessage>> GetNonPublishedOutboxMessages();
    }
}
