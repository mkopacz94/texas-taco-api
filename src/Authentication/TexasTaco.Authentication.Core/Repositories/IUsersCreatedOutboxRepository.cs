using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IUsersCreatedOutboxRepository
    {
        Task AddAsync(UserCreatedOutbox userCreatedOutbox);
        Task<IEnumerable<UserCreatedOutbox>> GetOutboxMessagesToBePublishedAsync();
        Task UpdateInTransactionAsync(
            UserCreatedOutbox userCreatedOutbox, Func<Task> taskToBeDoneInTransaction);
    }
}
