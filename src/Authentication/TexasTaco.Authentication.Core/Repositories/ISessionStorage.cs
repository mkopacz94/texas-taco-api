using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface ISessionStorage
    {
        Task<SessionId> CreateSession();
        Task<Session>? GetSession(SessionId sessionId);
    }
}
