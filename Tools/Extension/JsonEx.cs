using Newtonsoft.Json;

namespace Tools.Extension
{
    /// <summary>
    /// JSON 扩展
    /// </summary>
    public static class JsonEx
    {
        /// <summary>
        /// object to json string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T data) where T : class, new()
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// json string to model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string json) where T :class, new()
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
