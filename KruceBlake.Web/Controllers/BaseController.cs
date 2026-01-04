using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace KruceBlake.Web.Controllers
{
    [Obsolete("Not currently in use. Please use the default base AspNetCore.Mvc.Controller.")]
    /// <summary>
    /// All controllers should inherit from this to use logger. Gets service as a property instead using httpcontext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        //Note: Since these are used as properties and we're getting the service from our httpcontext.. keep in mind we can't access this until httpcontext is available in the pipeline 
        [AllowNull]
        protected ILogger<T>? Logger => field ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}