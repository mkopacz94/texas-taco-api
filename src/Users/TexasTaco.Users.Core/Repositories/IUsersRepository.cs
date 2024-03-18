using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Repositories
{
    public interface IUsersRepository
    {
        Task AddUserAsync(User user);
    }
}
