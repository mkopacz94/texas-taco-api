using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Products.Core.Repositories
{
    internal class ProductPriceChangedOutboxMessagesRepository(ProductsDbContext _dbContext)
        : IProductPriceChangedOutboxMessagesRepository
    {
        public async Task AddAsync(ProductPriceChangedOutboxMessage outboxMessage)
        {
            await _dbContext.AddAsync(outboxMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductPriceChangedOutboxMessage>> GetNonPublishedOutboxMessages()
        {
            return await _dbContext.ProductPriceChangedOutboxMessages
                .Where(uo => uo.MessageStatus == OutboxMessageStatus.ToBePublished)
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductPriceChangedOutboxMessage outboxMessage)
        {
            _dbContext.ProductPriceChangedOutboxMessages.Update(outboxMessage);
            await _dbContext.SaveChangesAsync();
        }
    }
}
