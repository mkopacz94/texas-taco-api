namespace TexasTaco.Orders.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task ExecuteTransactionAsync(
            Func<Task> action,
            CancellationToken cancellationToken = default);
    }
}
