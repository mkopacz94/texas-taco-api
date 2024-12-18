using Microsoft.EntityFrameworkCore;
using TexasTaco.Shared.Inbox;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    internal class PointsCollectedInboxMessagesRepository(
        UsersDbContext context)
        : IPointsCollectedInboxMessagesRepository
    {
        private readonly UsersDbContext _context = context;

        public async Task AddAsync(PointsCollectedInboxMessage message)
        {
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PointsCollectedInboxMessage>> GetNonProcessedMessages()
        {
            return await _context
                .PointsCollectedInboxMessages
                .Where(m => m.MessageStatus == InboxMessageStatus.ToBeProcessed)
                .ToListAsync();
        }

        public async Task UpdateAsync(PointsCollectedInboxMessage message)
        {
            _context.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ContainsMessageWithSameId(Guid id)
        {
            return await _context
                .PointsCollectedInboxMessages
                .AnyAsync(m => m.MessageId.ToString() == id.ToString());
        }
    }
}
