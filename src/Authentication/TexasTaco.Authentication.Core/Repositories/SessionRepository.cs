using Microsoft.Extensions.Caching.Distributed;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class SessionRepository(IDistributedCache authCache) : ISessionRepository
    {
        private readonly IDistributedCache _authCache = authCache;

        public Task<Guid> CreateSession()
        {
            return Task.FromResult(Guid.NewGuid());
        }

        public Task<Session> GetSession(Guid sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
