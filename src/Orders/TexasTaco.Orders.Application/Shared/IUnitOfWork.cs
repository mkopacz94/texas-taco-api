namespace TexasTaco.Orders.Application.Shared
{
    public interface IUnitOfWork
    {
        Task ExecuteTransactionAsync(
            Func<Task> action,
            CancellationToken cancellationToken = default);
    }
}
