using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KruceBlake.Api.Utilities
{
    public static class JsonFileUtils
    {
        public static void WriteDynamicJsonObject(JObject jsonObj, string fileName)
        {
            using var streamWriter = File.CreateText(fileName);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            jsonObj.WriteTo(jsonWriter);
        }
    }
}
