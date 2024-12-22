namespace TexasTaco.Shared.Outbox.Repository
{
    public interface IOutboxMessagesRepository<T>
    {
        Task AddAsync(T message);
        Task UpdateAsync(T message);
        Task<IEnumerable<T>> GetNonPublishedMessages();
    }
}
