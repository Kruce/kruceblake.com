using KruceBlake.Api.Exceptions;
using KruceBlake.Api.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace KruceBlake.Api.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var kruceBlakeApiOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<KruceBlakeApiOptions>>();

            var kruceBlakeApi = kruceBlakeApiOptions.Value ?? 
                throw new InternalServerErrorException("API options are not configured. Please notify the admin.");

            if (!context.HttpContext.Request.Headers.TryGetValue(kruceBlakeApi.HeaderName, out var extractedApiKey))
                throw new UnauthorizedException("API Key was not provided");

            if (!extractedApiKey.Equals(kruceBlakeApi.Key))
                throw new UnauthorizedException("API Key is not valid");

            await next();
        }
    }
}