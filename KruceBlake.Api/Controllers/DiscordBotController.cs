using KruceBlake.Api.Attributes;
using KruceBlake.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace KruceBlake.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DiscordBotController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DiscordBotController> _logger;
        public DiscordBotController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory, ILogger<DiscordBotController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Wake()
        {
            int attempt = 0;
            int maxAttempts = 10;
            bool isRunning = false;
            HttpClient client;
            HttpRequestMessage request;
            HttpResponseMessage response;

            //1. ping the bot to wake it up, or keep pinging a max of 10 times until it is awake
            while (!isRunning && attempt < maxAttempts)
            {
                attempt++;
                try
                {
                    client = _httpClientFactory.CreateClient();
                    response = await client.GetAsync("https://discord-bot-kruceblake.koyeb.app/");
                    isRunning = response.IsSuccessStatusCode;

                    if (!isRunning)
                        await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "there was an error during sending a get request to koyeb.");
                    return StatusCode(502, "there was an error pinging the bot service. please inform the administrator.");
                }
            }

            if (!isRunning)
            {
                Response.Headers.Append(HeaderNames.RetryAfter, "120"); //2 mins
                _logger.LogError($"a get request was sent to koyeb {maxAttempts} times and is unsuccessful.");
                return StatusCode(503, $"bot was pinged {maxAttempts} times and is either still asleep or waking up. please retry in a couple minutes.");
            }

            //2. check if the cron-job service that automatically pings the bot is still enabled or not
            bool isEnabled;
            try
            {
                client = _httpClientFactory.CreateClient("CronJob");
                response = await client.GetAsync("/jobs/5913430");

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(body);
                    isEnabled = Convert.ToBoolean(data["jobDetails"]?["enabled"] ?? bool.FalseString);
                }
                else
                {
                    _logger.LogError("sending a get request to cron-job to get its job details returned an unsuccessful response.");
                    return StatusCode(502, "there was a failure retrieving the discord bot's cron-job enabled status.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "there was an error during sending a get request to cron-job to get its job details.");
                return StatusCode(503, "there was a error retrieving the discord bot's cron-job enabled status.");
            }

            //3. if the cron-job service that automatically pings the bot is not enabled, re-enable it
            if (!isEnabled)
            {
                try
                {
                    request = new(HttpMethod.Patch, "/jobs/5913430")
                    {
                        Content = new StringContent("{\"job\":{\"enabled\":true}}")
                    };
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("sending a patch request to cron-job to set enabled equal to true returned an unsuccessful response.");
                        return StatusCode(502, "there was a failure enabling the discord bot's cron-job.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "there was an error during sending a patch request to cron-job to set enabled equal to true.");
                    return StatusCode(502, "there was an error enabling the discord bot's cron-job.");
                }
            }

            return Ok("bot is awake and cron-job service is enabled.");
        }

        [HttpGet]
        public IActionResult GetBookmarks()
        {
            return GetJson("bookmarks");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutBookmarks([FromBody] JObject json)
        {
            return UpdateJson("bookmarks", json);
        }
        [HttpGet]
        public IActionResult GetReminders()
        {
            return GetJson("reminders");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutReminders([FromBody] JObject json)
        {
            return UpdateJson("reminders", json);
        }
        [HttpGet]
        public IActionResult GetCountdowns()
        {
            return GetJson("countdowns");
        }
        [ApiKey]
        [HttpPut]
        public IActionResult PutCountdowns([FromBody] JObject json)
        {
            return UpdateJson("countdowns", json);
        }
        private IActionResult GetJson(string fileName)
        {
            var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"data\\{fileName}.json");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            var data = System.IO.File.ReadAllText(fullPath);
            return Content(data, "application/json");
        }
        private IActionResult UpdateJson(string fileName, JObject json)
        {
            var fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, $"data\\{fileName}.json");
            if (!Path.Exists(fullPath))
            {
                return NotFound();
            }
            JsonWriterHelper.WriteDynamicJsonObject(json, fullPath);
            return Ok(json);
        }
    }
}
