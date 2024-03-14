using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Services
{
    internal class SessionStorage(IDistributedCache _sessionsCache) : ISessionStorage
    {
        public async Task<SessionId> CreateSession(DateTime expirationDate)
        {
            var sessionId = new SessionId(Guid.NewGuid());
            var session = new Session(DateTime.UtcNow, expirationDate);

            await _sessionsCache.SetStringAsync(
                sessionId.Value.ToString(),
                JsonConvert.SerializeObject(session));

            return sessionId;
        }

        public async Task<Session?> GetSession(SessionId sessionId)
        {
            string? sessionString = await _sessionsCache
                .GetStringAsync(sessionId.Value.ToString());

            if (string.IsNullOrWhiteSpace(sessionString))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Session?>(sessionString);
        }

        public async Task UpdateSession(SessionId sessionId, Session session)
        {
            await _sessionsCache.SetStringAsync(
                sessionId.Value.ToString(),
                JsonConvert.SerializeObject(session));
        }
    }
}
