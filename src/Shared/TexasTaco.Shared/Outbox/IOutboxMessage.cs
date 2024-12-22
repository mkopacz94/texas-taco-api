namespace TexasTaco.Shared.Outbox
{
    public interface IOutboxMessage
    {
        OutboxMessageStatus MessageStatus { get; }
    }
}
