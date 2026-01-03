using KruceBlake.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace KruceBlake.Api.Handlers
{
    public partial class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Error",
                Detail = "An error has occurred. Contact the administrator with the reference id and date this occurred for assistance.",
                Instance = httpContext.Request.Path
            };

            if (exception is BaseException e)
            {
                var statusCodeInt = (int)e.StatusCode;
                problemDetails.Title = ReasonPhrases.GetReasonPhrase(statusCodeInt);
                httpContext.Response.StatusCode = statusCodeInt;
            }

            problemDetails.Status = httpContext.Response.StatusCode;

            var id = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            problemDetails.Extensions.Add("referenceId", id);

            LogError(id, exception.Message, exception);

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        [LoggerMessage(
             EventId = 0,
             Level = LogLevel.Error,
             Message = "Reference ID: {requestId} Message: {message}")]
        private partial void LogError(string requestId, string message, Exception exception);
    }
}