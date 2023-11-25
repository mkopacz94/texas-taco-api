using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface ISessionRepository
    {
        Task<Guid> CreateSession();
        Task<Session> GetSession(Guid sessionId);
    }
}
