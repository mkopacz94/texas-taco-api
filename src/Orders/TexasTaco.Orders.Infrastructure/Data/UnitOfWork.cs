using TexasTaco.Orders.Application.UnitOfWork;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data
{
    internal class UnitOfWork(OrdersDbContext _dbContext) : IUnitOfWork
    {
        public async Task ExecuteTransactionAsync(
            Func<Task> action,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext
                .Database
                .BeginTransactionAsync(cancellationToken);

            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
