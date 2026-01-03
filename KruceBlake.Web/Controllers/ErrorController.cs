using KruceBlake.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.Net;

namespace KruceBlake.Web.Controllers
{
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public partial class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            var model = GetErrorViewModel("error", "sorry about that..", (HttpStatusCode)HttpContext.Response.StatusCode);
            UpdatePathAndLogErrors(model);
            return View(model);
        }

        [Route("Error/{statusCodeInt}")]
        public IActionResult Error(int statusCodeInt)
        {
            var model = GetErrorViewModel(statusCodeInt.ToString(), ReasonPhrases.GetReasonPhrase(statusCodeInt), (HttpStatusCode)statusCodeInt);
            UpdatePathAndLogErrors(model);
            return View(model);
        }

        private ErrorViewModel GetErrorViewModel(string errorTitle, string message, HttpStatusCode statusCode) => new()
        {
            Title = errorTitle,
            Message = message,
            StatusCode = statusCode,
            ReferenceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        /// <summary>
        /// Logs exceptions if any exist in the exception handler path feature as well as status code re-execute features and updates the original path in the error view model.
        /// </summary>
        /// <param name="model"></param>
        private void UpdatePathAndLogErrors(ErrorViewModel model)
        {
            //Log original error if one exists
            var features = HttpContext.Features;
            var exceptionHandlerFeature = features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerFeature != null)
            {
                model.OriginalPath = exceptionHandlerFeature.Path;
                LogError(model.ReferenceId, model.OriginalPath, exceptionHandlerFeature.Error);
            }
            //Log re-executed status code if one exists
            var reExecuteFeature = features.Get<IStatusCodeReExecuteFeature>();
            if (reExecuteFeature != null)
            {
                model.OriginalPath = reExecuteFeature.OriginalPath;
                LogErrorStatusCode(model.ReferenceId, model.OriginalPath, model.StatusCode);
            }
            model.OriginalPath ??= "unknown";
        }
    }
}