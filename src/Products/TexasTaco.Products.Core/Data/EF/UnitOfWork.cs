using Microsoft.EntityFrameworkCore.Storage;

namespace TexasTaco.Products.Core.Data.EF
{
    internal class UnitOfWork(ProductsDbContext _dbContext) : IUnitOfWork
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
