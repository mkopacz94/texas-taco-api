using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Clients
{
    public class AuthenticationClient(
        HttpClient _client,
        ILogger<AuthenticationClient> _logger) : IAuthenticationClient
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
                _logger.LogError("Session validation endpoint returned " +
                    "no success status code ({statusCode}). Details: {error}", response.StatusCode, response.Content);
                return null;
            }

            var session = await response.Content.ReadFromJsonAsync<Session>();

            return session;
        }
    }
}
