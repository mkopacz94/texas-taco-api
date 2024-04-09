using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Services
{
    public interface ISessionStorage
    {
        Task<SessionId> CreateSession(AccountId accountId, DateTime expirationDate);
        Task<Session?> GetSession(AccountId accountId, SessionId sessionId);
        Task UpdateSession(AccountId accountId, Session session);
        Task<List<Session>?> GetAccountSessions(AccountId accountId);
    }
}
