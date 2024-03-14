using Microsoft.Net.Http.Headers;

namespace TexasTaco.Api.Gateway.Clients.Handlers
{
    internal class CopyCookiesHandler(
        IHttpContextAccessor _contextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cookiesHeaders = _contextAccessor.HttpContext!.Request.Cookies
                .Select(c => $"{c.Key}={c.Value}");

            request.Headers.Add(HeaderNames.Cookie, string.Join("; ", cookiesHeaders));

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
