using KruceBlake.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace KruceBlake.Web.Controllers
{
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorController(ILogger<ErrorController> logger) : Controller
    {
        private readonly ILogger<ErrorController> _logger = logger;

        [Route("Error")]
        public IActionResult Error()
        {
            var model = GetErrorViewModel("error", "sorry about that..");
            _logger.LogError($"Exception #{model.RequestId}", this);
            return View(model);
        }

        [Route("Error/{statusCodeInt}")]
        public IActionResult Error(int statusCodeInt)
        {
            var model = GetErrorViewModel(statusCodeInt.ToString(), ReasonPhrases.GetReasonPhrase(statusCodeInt));
            _logger.LogError($"Exception #{model.RequestId}, Orignal path: {model.OriginalPath}", this);
            return View(model);
        }

        private ErrorViewModel GetErrorViewModel(string errorTitle, string message) => new()
        {
            Title = errorTitle,
            Message = message,
            OriginalPath = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IStatusCodeReExecuteFeature>()?.OriginalPath ?? "unknown",
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
    }
}
