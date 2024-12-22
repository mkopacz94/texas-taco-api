using Microsoft.EntityFrameworkCore;

namespace TexasTaco.Shared.Outbox.Repository
{
    public class OutboxMessagesRepository<TEntity>(
        DbContext context)
        : IOutboxMessagesRepository<TEntity>
        where TEntity : class, IOutboxMessage
    {
        private readonly DbContext _context = context;

        public async Task AddAsync(TEntity message)
        {
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetNonPublishedMessages()
        {
            return await _context
                .Set<TEntity>()
                .Where(m => m.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }

        public async Task UpdateAsync(TEntity message)
        {
            _context
               .Set<TEntity>()
               .Update(message);

            await _context.SaveChangesAsync();
        }
    }
}
