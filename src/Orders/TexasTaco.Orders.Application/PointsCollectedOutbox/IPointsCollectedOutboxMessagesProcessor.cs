namespace TexasTaco.Orders.Application.PointsCollectedOutbox
{
    public interface IPointsCollectedOutboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
