using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetByAccountIdAsync(Guid accountId);
        Task<User?> GetByIdAsync(UserId id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteByAccountIdAsync(AccountId id);
    }
}
