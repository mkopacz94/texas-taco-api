using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.ProductPriceChangedInbox;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Orders.Persistence.ProductPriceChangedInbox;
using TexasTaco.Shared.Inbox;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class ProductPriceChangedInboxMessagesRepository(OrdersDbContext _dbContext)
        : IProductPriceChangedInboxMessagesRepository
    {
        public async Task AddAsync(ProductPriceChangedInboxMessage message)
        {
            await _dbContext.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductPriceChangedInboxMessage>> GetNonProcessedMessages()
        {
            return await _dbContext
                .ProductPriceChangedInboxMessages
                .Where(m => m.MessageStatus == InboxMessageStatus.ToBeProcessed)
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductPriceChangedInboxMessage message)
        {
            _dbContext.Update(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ContainsMessageWithSameId(Guid id)
        {
            return await _dbContext
                .UserUpdatedInboxMessages
                .AnyAsync(m => m.MessageId.ToString() == id.ToString());
        }
    }
}
