namespace TexasTaco.Shared.Outbox
{
    public class OutboxMessage<TMessage>(TMessage messageBody) : IOutboxMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public TMessage MessageBody { get; private set; } = messageBody;
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public DateTime Published { get; private set; }
        public OutboxMessageStatus MessageStatus { get; private set; }
            = OutboxMessageStatus.ToBePublished;

        public void MarkAsPublished()
        {
            Published = DateTime.UtcNow;
            MessageStatus = OutboxMessageStatus.Published;
        }
    }
}
