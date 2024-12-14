using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.PointsCollectedOutbox;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Orders.Persistence.PointsCollectedOutboxMessages;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class PointsCollectedOutboxMessagesRepository(
        OrdersDbContext context)
        : IPointsCollectedOutboxMessagesRepository
    {
        private readonly OrdersDbContext _context = context;

        public async Task AddAsync(PointsCollectedOutboxMessage outboxMessage)
        {
            await _context.AddAsync(outboxMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PointsCollectedOutboxMessage>> GetNonPublishedOutboxMessages()
        {
            return await _context.PointsCollectedOutboxMessages
                .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }

        public async Task UpdateAsync(PointsCollectedOutboxMessage outboxMessage)
        {
            _context.PointsCollectedOutboxMessages.Update(outboxMessage);
            await _context.SaveChangesAsync();
        }
    }
}
