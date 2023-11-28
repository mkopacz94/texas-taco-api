using Microsoft.Extensions.Caching.Distributed;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class SessionStorage(IDistributedCache authCache) : ISessionStorage
    {
        private readonly IDistributedCache _authCache = authCache;
        private readonly Dictionary<Guid, Session> _sessions = [];

        public Task<SessionId> CreateSession()
        {
            var sessionId = Guid.NewGuid();
            var session = new Session(DateTime.UtcNow.AddMinutes(2));
            _sessions.Add(sessionId, session);

            return Task.FromResult(new SessionId(sessionId));
        }

        public Task<Session>? GetSession(SessionId sessionId)
        {
            if(_sessions.TryGetValue(sessionId.Value, out Session? session))
            {
                return Task.FromResult(session);
            }

            return null;
        }
    }
}
