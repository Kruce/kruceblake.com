using Microsoft.AspNetCore.Mvc;

namespace KruceBlake.Api.Controllers
{
    public partial class DiscordBotController : ControllerBase
    {
        public readonly ILogger<DiscordBotController> _logger;
        public partial DiscordBotController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory, ILogger<DiscordBotController> logger);

        [LoggerMessage(
             EventId = 0,
             Level = LogLevel.Error,
             Message = "there was an error during sending a get request to koyeb: {errorMessage}")]
        private partial void LogErrorKoyeb(string errorMessage, Exception exception);

        [LoggerMessage(
             EventId = 1,
             Level = LogLevel.Error,
             Message = "there was an error during sending a get request to cron-job to get its job details: {errorMessage}")]
        private partial void LogErrorCronJobDetails(string errorMessage, Exception exception);

        [LoggerMessage(
             EventId = 2,
             Level = LogLevel.Error,
             Message = "there was an error during sending a patch request to cron-job to set enabled equal to true: {errorMessage}")]
        private partial void LogErrorCronJobPatch(string errorMessage, Exception exception);
    }
}
