namespace TexasTaco.Api.Gateway.Services
{
    public interface ICookieService
    {
        void SetCookie(string cookieName, string value, CookieOptions options);
        string? GetCookie(string cookieName);
    }
}
