using KruceBlake.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KruceBlake.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new IndexViewModel());
        }
    }
}