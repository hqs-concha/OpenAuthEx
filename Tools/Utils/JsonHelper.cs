using Newtonsoft.Json;

namespace Tools.Utils
{
    public class JsonHelper
    {
        public static string ToJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static object ToObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
