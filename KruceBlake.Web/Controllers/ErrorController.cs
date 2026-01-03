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
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (feature is null) //attempting to access /Error directly
                return NotFound();

            var model = GetErrorViewModel("error", "sorry about that..", feature.Path, (HttpStatusCode)HttpContext.Response.StatusCode);
            LogError(model.ReferenceId, model.OriginalPath, feature.Error);
            return View(model);
        }

        [Route("Error/{statusCodeInt}")]
        public IActionResult Error(int statusCodeInt)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (feature is null) //attempting to access /Error/{statusCodeInt} directly
                return NotFound();

            var model = GetErrorViewModel(statusCodeInt.ToString(), ReasonPhrases.GetReasonPhrase(statusCodeInt), feature.OriginalPath, (HttpStatusCode)statusCodeInt);
            LogErrorStatusCode(model.ReferenceId, model.OriginalPath, model.StatusCode);
            return View(model);
        }

        private ErrorViewModel GetErrorViewModel(string title, string message, string path, HttpStatusCode statusCode) => new()
        {
            Title = title,
            Message = message,
            OriginalPath = path,
            StatusCode = statusCode,
            ReferenceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
    }
}