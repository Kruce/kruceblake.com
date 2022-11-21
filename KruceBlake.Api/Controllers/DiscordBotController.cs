using KruceBlake.Api.Attributes;
using KruceBlake.Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace KruceBlake.Api.Controllers
{
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
            return ReadJson("bookmarks.json");
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
            return ReadJson("reminders.json");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutReminders([FromBody] JObject json)
        {
            return UpdateJson("reminders.json", json);
        }
        private IActionResult ReadJson(string fileName)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            var fullPath = Path.Combine(rootPath, $"data\\{fileName}");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            var data = System.IO.File.ReadAllText(fullPath);
            return Ok(data);
        }
        private IActionResult UpdateJson(string fileName, JObject json)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            var fullPath = Path.Combine(rootPath, $"data\\{fileName}");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            JsonFileUtils.WriteDynamicJsonObject(json, fullPath);
            return Ok(json);
        }
    }
}
