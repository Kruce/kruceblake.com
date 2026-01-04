using KruceBlake.Api.Exceptions;
using KruceBlake.Api.Options;
using Microsoft.Extensions.Options;

namespace KruceBlake.Api.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IOptions<KruceBlakeApiOptions> options)
    {
        private readonly RequestDelegate _next = next;
        private readonly KruceBlakeApiOptions _options = options.Value;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var extractedApiKey))
                throw new UnauthorizedException("API Key was not provided");

            if (!extractedApiKey.Equals(_options.Key))
                throw new UnauthorizedException("API Key is not valid");

            await _next(context);
        }
    }
}