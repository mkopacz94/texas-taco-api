using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Services
{
    public interface ISessionStorage
    {
        Task<SessionId> CreateSession(DateTime expirationDate);
        Task<Session?> GetSession(SessionId sessionId);
        Task UpdateSession(SessionId sessionId, Session session);
    }
}
