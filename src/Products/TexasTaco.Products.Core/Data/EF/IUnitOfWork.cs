using Microsoft.EntityFrameworkCore.Storage;

namespace TexasTaco.Products.Core.Data.EF
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default);
    }
}
