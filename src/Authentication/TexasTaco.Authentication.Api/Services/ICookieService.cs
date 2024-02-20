namespace TexasTaco.Authentication.Api.Services
{
    public interface ICookieService
    {
        void SetCookie(string cookieName, string value, CookieOptions options);
        string? GetCookie(string cookieName);
    }
}
