using Microsoft.EntityFrameworkCore.Storage;

namespace TexasTaco.Users.Core.Data.EF
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default);
    }
}
