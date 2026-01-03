using KruceBlake.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Versioning;

namespace KruceBlake.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var attr = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
                .SingleOrDefault() as TargetFrameworkAttribute;

            var model = new IndexViewModel()
            {
                CurrentFramework = string.IsNullOrEmpty(attr?.FrameworkName) ? ".NET" : $"{attr?.FrameworkName} ({attr?.FrameworkDisplayName})"
            };
            return View(model);
        }
    }
}