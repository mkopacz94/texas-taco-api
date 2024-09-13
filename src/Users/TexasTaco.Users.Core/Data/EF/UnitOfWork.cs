using Microsoft.EntityFrameworkCore.Storage;

namespace TexasTaco.Users.Core.Data.EF
{
    internal class UnitOfWork(UsersDbContext _dbContext) : IUnitOfWork
    {
        private IDbContextTransaction? _dbTransaction;

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            _dbTransaction = await _dbContext
                .Database
                .BeginTransactionAsync(cancellationToken);

            return _dbTransaction;
        }
    }
}
