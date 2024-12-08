using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IProductPriceChangedOutboxMessagesRepository
    {
        Task AddAsync(ProductPriceChangedOutboxMessage outboxMessage);
        Task UpdateAsync(ProductPriceChangedOutboxMessage outboxMessage);
        Task<IEnumerable<ProductPriceChangedOutboxMessage>> GetNonPublishedOutboxMessages();
    }
}
