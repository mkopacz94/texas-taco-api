namespace TexasTaco.Shared.Inbox
{
    public interface IInboxMessage
    {
        Guid Id { get; }
        Guid MessageId { get; }
        InboxMessageStatus MessageStatus { get; }
    }
}
