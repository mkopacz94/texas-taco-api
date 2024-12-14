using TexasTaco.Orders.Persistence.PointsCollectedOutboxMessages;

namespace TexasTaco.Orders.Application.PointsCollectedOutbox
{
    public interface IPointsCollectedOutboxMessagesRepository
    {
        Task AddAsync(PointsCollectedOutboxMessage outboxMessage);
        Task UpdateAsync(PointsCollectedOutboxMessage outboxMessage);
        Task<IEnumerable<PointsCollectedOutboxMessage>> GetNonPublishedOutboxMessages();
    }
}
