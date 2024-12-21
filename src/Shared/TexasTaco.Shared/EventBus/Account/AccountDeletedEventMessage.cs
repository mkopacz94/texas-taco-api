namespace TexasTaco.Shared.EventBus.Account
{
    public sealed record AccountDeletedEventMessage(
        Guid Id,
        Guid AccountId);
}
