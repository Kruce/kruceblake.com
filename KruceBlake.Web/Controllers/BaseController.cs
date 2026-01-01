using Microsoft.AspNetCore.Mvc;

namespace KruceBlake.Web.Controllers
{
    [Obsolete("Not currently in use. Please use the default base AspNetCore.Mvc.Controller.")]
    /// <summary>
    /// All controllers should inherit from this to use logger. Gets service as a property instead using httpcontext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : Controller where T : BaseController<T>
    {
        private ILogger<T> _logger;
        //Note: Since these are used as properties and we're getting the service from our httpcontext.. keep in mind we can't access this until httpcontext is available in the pipeline 
        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}