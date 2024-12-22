namespace TexasTaco.Shared.Inbox
{
    public class InboxMessage<TMessage>(
        TMessage messageBody)
        : IInboxMessage
        where TMessage : IInboxMessageBody
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Received { get; private set; } = DateTime.UtcNow;
        public DateTime Processed { get; private set; }
        public Guid MessageId { get; private set; } = messageBody.Id;
        public TMessage MessageBody { get; } = messageBody;
        public InboxMessageStatus MessageStatus { get; private set; }

        public void MarkAsProcessed()
        {
            Processed = DateTime.UtcNow;
            MessageStatus = InboxMessageStatus.Processed;
        }
    }
}
