using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IPointsCollectedInboxMessagesRepository
    {
        Task AddAsync(PointsCollectedInboxMessage message);
        Task UpdateAsync(PointsCollectedInboxMessage message);
        Task<IEnumerable<PointsCollectedInboxMessage>> GetNonProcessedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
