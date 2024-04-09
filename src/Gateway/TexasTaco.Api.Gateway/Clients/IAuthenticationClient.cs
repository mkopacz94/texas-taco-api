using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Clients
{
    public interface IAuthenticationClient
    {
        Task<Session?> GetSession(string? accountId, string? sessionId);
    }
}
