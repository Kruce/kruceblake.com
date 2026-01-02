using Microsoft.AspNetCore.Mvc;

namespace KruceBlake.Web.Controllers
{
    public partial class ErrorController(ILogger<ErrorController> logger) : Controller
    {
        /// <summary>
        /// Log a general error with the included request ID.
        /// </summary>
        /// <param name="requestId"></param>
        [LoggerMessage(
            EventId = 0, 
            Level = LogLevel.Error, 
            Message = "An exception has occurred. Request ID: {requestId}")]
        private partial void LogError(string requestId);

        /// <summary>
        /// Log a general status code error with the included request ID and original path.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="originalPath"></param>
        [LoggerMessage(
            EventId = 1, 
            Level = LogLevel.Error, 
            Message = "An exception has occurred. Request ID: {requestId}, Orignal path: {originalPath}")]
        private partial void LogErrorStatusCode(string requestId, string originalPath);
    }
}