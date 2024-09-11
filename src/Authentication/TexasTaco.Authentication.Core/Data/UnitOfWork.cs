using Microsoft.EntityFrameworkCore.Storage;
using TexasTaco.Authentication.Core.Data.EF;

namespace TexasTaco.Authentication.Core.Data
{
    internal class UnitOfWork(AuthDbContext _dbContext) : IUnitOfWork
    {
        private IDbContextTransaction? _dbTransaction;

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _dbTransaction = await _dbContext
                .Database
                .BeginTransactionAsync(cancellationToken);

            return _dbTransaction;
        }
    }
}
