using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Services
{
    internal class SessionStorage(IDistributedCache _sessionsCache) : ISessionStorage
    {
        public async Task<SessionId> CreateSession(AccountId accountId, DateTime expirationDate)
        {
            var newSession = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow,
                expirationDate,
                false);

            var cachedAccountSessions = await GetAccountSessions(accountId);

            if (cachedAccountSessions is null)
            {
                var accountSessions = new List<Session>
                {
                    newSession
                };

                await _sessionsCache.SetStringAsync(
                    accountId.Value.ToString(),
                    JsonConvert.SerializeObject(accountSessions));

                return newSession.Id;
            }

            cachedAccountSessions.Add(newSession);

            await _sessionsCache.SetStringAsync(
                accountId.Value.ToString(),
                JsonConvert.SerializeObject(cachedAccountSessions));

            return newSession.Id;
        }

        public async Task<Session?> GetSession(AccountId accountId, SessionId sessionId)
        {
            var accountSessions = await GetAccountSessions(accountId);

            if (accountSessions is null)
            {
                return null;
            }

            return accountSessions
                .FirstOrDefault(s => s.Id == sessionId);
        }

        public async Task UpdateSession(AccountId accountId, Session session)
        {
            var accountSessions = await GetAccountSessions(accountId);

            if (accountSessions is null)
            {
                return;
            }

            var sessionToUpdate = accountSessions
                .First(s => s.Id == session.Id);

            sessionToUpdate.Update(session);

            await _sessionsCache.SetStringAsync(
                accountId.Value.ToString(),
                JsonConvert.SerializeObject(accountSessions));
        }

        public async Task<List<Session>?> GetAccountSessions(AccountId accountId)
        {
            string? accountSessionsString = await _sessionsCache
                .GetStringAsync(accountId.Value.ToString());

            if (string.IsNullOrWhiteSpace(accountSessionsString))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<List<Session>?>(accountSessionsString);
        }
    }
}
