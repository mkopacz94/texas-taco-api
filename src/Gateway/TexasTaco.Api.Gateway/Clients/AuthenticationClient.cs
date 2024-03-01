using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Clients
{
    public class AuthenticationClient(HttpClient _client)
    {
        public async Task<Session?> GetSession(string? sessionId)
        {
            if(string.IsNullOrWhiteSpace(sessionId))
            {
                return null;
            }

            var session = await _client
                .GetFromJsonAsync<Session>($"session-valid?sessionId={sessionId}");

            return session;
        }
    }
}
