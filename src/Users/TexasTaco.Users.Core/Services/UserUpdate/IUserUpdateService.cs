using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Services.UserUpdate
{
    public interface IUserUpdateService
    {
        Task UpdateUser(User user);
    }
}
