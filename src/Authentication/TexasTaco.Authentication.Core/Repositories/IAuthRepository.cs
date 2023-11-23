using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<Guid> CreateSession();
        Task<Session> GetSession(Guid sessionId);
    }
}
