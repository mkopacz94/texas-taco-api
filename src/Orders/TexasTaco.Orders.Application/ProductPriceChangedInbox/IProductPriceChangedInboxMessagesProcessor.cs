namespace TexasTaco.Orders.Application.ProductPriceChangedInbox
{
    public interface IProductPriceChangedInboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
