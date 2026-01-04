using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KruceBlake.Web.Controllers
{
    public partial class ErrorController(ILogger<ErrorController> logger) : Controller
    {
        /// <summary>
        /// Log a general error with the included trace ID.
        /// </summary>
        /// <param name="traceId"></param>
        [LoggerMessage(
            EventId = 0, 
            Level = LogLevel.Error, 
            Message = "An exception has occurred. Trace ID: {traceId}, Path: {path}")]
        private partial void LogError(string traceId, string path, Exception exception);

        /// <summary>
        /// Log a general status code error with the included trace ID and original path.
        /// </summary>
        /// <param name="traceId"></param>
        /// <param name="originalPath"></param>
        [LoggerMessage(
            EventId = 1, 
            Level = LogLevel.Error, 
            Message = "An exception has occurred. Trace ID: {traceId}, Orignal path: {originalPath}, Status Code: {statusCode}")]
        private partial void LogErrorStatusCode(string traceId, string originalPath, HttpStatusCode statusCode);
    }
}