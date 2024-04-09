using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Clients
{
    public class AuthenticationClient(HttpClient _client) : IAuthenticationClient
    {
        public async Task<Session?> GetSession(string? accountId, string? sessionId)
        {
            if(string.IsNullOrWhiteSpace(sessionId))
            {
                return null;
            }

            var response = await _client.GetAsync($"session-valid?accountId={accountId}&sessionId={sessionId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var session = await response.Content.ReadFromJsonAsync<Session>();

            return session;
        }
    }
}
