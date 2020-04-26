using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tools.Extension;

namespace Tools.Utils
{
    /// <summary>
    /// HTTP 请求辅助类
    /// </summary>
    public class HttpHelper
    {
        private static HttpClient GetClient(Dictionary<string, object> headers = null)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30000);
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                }
            }
            return client;
        }

        /// <summary>
        /// Http Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, object> headers = null) where T : class, new()
        {
            var str = await GetStringAsync(url, headers);
            return str.ToModel<T>();
        }

        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns>string</returns>
        public static async Task<string> GetStringAsync(string url, Dictionary<string, object> headers = null)
        {
            var client = GetClient(headers);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            return str;
        }

        /// <summary>
        /// Http Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string url, Dictionary<string, object> param, Dictionary<string, object> headers = null) where T : class, new()
        {
            var str = await PostStringAsync(url, param, headers);
            return str.ToModel<T>();
        }

        /// <summary>
        /// Http Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> PostStringAsync(string url, Dictionary<string, object> param,
            Dictionary<string, object> headers = null)
        {
            var client = GetClient(headers);
            var uri = new Uri(url);
            var content = new StringContent(param.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
            var str = await response.Content.ReadAsStringAsync();
            return str;
        }
    }
}
