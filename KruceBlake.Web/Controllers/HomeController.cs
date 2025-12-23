using KruceBlake.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KruceBlake.Web.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel());
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
