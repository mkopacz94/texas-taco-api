namespace TexasTaco.Products.Core.Services
{
    public interface IProductPriceChangedOutboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
