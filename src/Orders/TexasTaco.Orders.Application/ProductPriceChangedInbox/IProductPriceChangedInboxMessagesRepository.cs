using TexasTaco.Orders.Persistence.ProductPriceChangedInbox;

namespace TexasTaco.Orders.Application.ProductPriceChangedInbox
{
    public interface IProductPriceChangedInboxMessagesRepository
    {
        Task AddAsync(ProductPriceChangedInboxMessage message);
        Task UpdateAsync(ProductPriceChangedInboxMessage message);
        Task<IEnumerable<ProductPriceChangedInboxMessage>> GetNonProcessedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
