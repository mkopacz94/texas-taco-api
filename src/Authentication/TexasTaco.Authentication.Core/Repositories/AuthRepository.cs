using Microsoft.Extensions.Caching.Distributed;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AuthRepository : IAuthRepository
    {
        private readonly IDistributedCache _authCache;

        public AuthRepository(IDistributedCache authCache)
        {
            _authCache = authCache;
        }

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
