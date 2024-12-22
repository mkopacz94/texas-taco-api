namespace TexasTaco.Shared.Inbox.Repository
{
    public interface IInboxMessagesRepository<TEntity>
    {
        Task AddAsync(TEntity message);
        Task UpdateAsync(TEntity message);
        Task<IEnumerable<TEntity>> GetNonProcessedMessages();
        Task<bool> ContainsMessageWithSameId(Guid id);
    }
}
