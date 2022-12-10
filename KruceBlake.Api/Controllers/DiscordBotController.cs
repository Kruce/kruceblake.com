using KruceBlake.Api.Attributes;
using KruceBlake.Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json.Linq;

namespace KruceBlake.Api.Controllers
{
    [EnableRateLimiting("Api")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class DiscordBotController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DiscordBotController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetBookmarks()
        {
            return GetJson("bookmarks.json");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutBookmarks([FromBody] JObject json)
        {
            return UpdateJson("bookmarks.json", json);
        }
        [HttpGet]
        public IActionResult GetReminders()
        {
            return GetJson("reminders.json");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutReminders([FromBody] JObject json)
        {
            return UpdateJson("reminders.json", json);
        }
        private IActionResult GetJson(string fileName)
        {
            var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"data\\{fileName}");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            var data = System.IO.File.ReadAllText(fullPath);
            return Ok(data);
        }
        private IActionResult UpdateJson(string fileName, JObject json)
        {
            var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"data\\{fileName}");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            JsonFileUtils.WriteDynamicJsonObject(json, fullPath);
            return Ok(json);
        }
    }
}
