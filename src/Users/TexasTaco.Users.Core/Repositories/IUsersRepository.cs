using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetByAccountIdAsync(Guid accountId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
