using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class SessionStorage(IDistributedCache _sessionsCache) : ISessionStorage
    {
        public async Task<SessionId> CreateSession(DateTime expirationDate)
        {
            var sessionId = new SessionId(Guid.NewGuid());
            var session = new Session(expirationDate);

            await _sessionsCache.SetStringAsync(
                sessionId.ToString(),
                JsonConvert.SerializeObject(session));

            return sessionId;
        }

        public async Task<Session?> GetSession(SessionId sessionId)
        {
            string? sessionString = await _sessionsCache
                .GetStringAsync(sessionId.Value.ToString());

            if(string.IsNullOrWhiteSpace(sessionString))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Session?>(sessionString);
        }
    }
}
