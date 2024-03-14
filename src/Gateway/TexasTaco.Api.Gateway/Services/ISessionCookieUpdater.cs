namespace TexasTaco.Api.Gateway.Services
{
    internal interface ISessionCookieUpdater
    {
        void UpdateSessionCookie(HttpContext context);
    }
}
