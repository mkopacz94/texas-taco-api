using System.Linq.Expressions;
using TexasTaco.Shared.Pagination;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<PagedResult<User>> GetPagedUsersAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<User, bool>>? filter);
        Task<User?> GetByAccountIdAsync(Guid accountId);
        Task<User?> GetByIdAsync(UserId id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteByAccountIdAsync(AccountId id);
    }
}
