using Microsoft.EntityFrameworkCore;

namespace TexasTaco.Shared.Inbox.Repository
{
    public class InboxMessagesRepository<TEntity>(
        DbContext context)
        : IInboxMessagesRepository<TEntity>
        where TEntity : class, IInboxMessage
    {
        private readonly DbContext _context = context;

        public async Task AddAsync(TEntity message)
        {
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ContainsMessageWithSameId(Guid id)
        {
            return await _context
               .Set<TEntity>()
               .AnyAsync(m => m.MessageId.ToString() == id.ToString());
        }

        public async Task<IEnumerable<TEntity>> GetNonProcessedMessages()
        {
            return await _context
                .Set<TEntity>()
                .Where(m => m.MessageStatus == InboxMessageStatus.ToBeProcessed)
                .ToListAsync();
        }

        public async Task UpdateAsync(TEntity message)
        {
            _context.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}
