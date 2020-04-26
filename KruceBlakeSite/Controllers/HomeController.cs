using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KruceBlakeSite.Models;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Linq;
using System.Runtime.Versioning;

namespace KruceBlakeSite.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            Logger.LogError($"Exception #{model.RequestId}", this);
            return View(model);
        }

        [AllowAnonymous]
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
