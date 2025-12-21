using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KruceBlake.Api.Helpers
{
    public static class JsonWriterHelper
    {
        public static void WriteDynamicJsonObject(JObject jsonObj, string fileName)
        {
            using var streamWriter = File.CreateText(fileName);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            jsonObj.WriteTo(jsonWriter);
        }
    }
}
