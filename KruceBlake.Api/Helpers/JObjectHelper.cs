using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KruceBlake.Api.Helpers
{
    public static class JObjectHelper
    {
        /// <summary>
        /// Write JObject to a file at the specific path using StreamWriter and JsonTextWriter
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <param name="path"></param>
        public static void WriteToFile(JObject jsonObj, string path)
        {
            using var file = File.CreateText(path);
            using var writer = new JsonTextWriter(file);
            jsonObj.WriteTo(writer);
        }
    }
}