using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Services
{
    internal class InMemoryCurrentSessionStorage : ICurrentSessionStorage
    {
        private Session? _session;

        public Session? GetSavedSessionFromStorage() => _session;

        public void SaveSessionInStorage(Session session) => _session = session;
    }
}
