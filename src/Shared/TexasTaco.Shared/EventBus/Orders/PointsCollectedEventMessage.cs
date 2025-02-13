using TexasTaco.Shared.Inbox;

namespace TexasTaco.Shared.EventBus.Orders
{
    public sealed record PointsCollectedEventMessage(
        Guid Id,
        Guid AccountId,
        int PointsCollected)
        : IInboxMessageBody;
}
