using Microsoft.EntityFrameworkCore.Storage;

namespace TexasTaco.Authentication.Core.Data
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
