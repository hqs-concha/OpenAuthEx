

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Tools.Extension;
using Tools.Utils;

namespace Mvc.Core.Utils
{
    /// <summary>
    /// 防重放 辅助类
    /// </summary>
    internal class AntiReplayHelper
    {
        /// <summary>
        /// 验证请求头参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        internal static bool VerifyHeader(HttpRequest request, IMemoryCache cache)
        {
            var headers = request.Headers;
            if (string.IsNullOrEmpty(headers["X-CA-Key"]))
                return false;

            if (string.IsNullOrEmpty(headers["X-CA-NONCE"]))
                return false;

            if (string.IsNullOrEmpty(headers["X-CA-SIGNATURE"]))
                return false;

            if (string.IsNullOrEmpty(headers["X-CA-TIMESTAMP"]))
                return false;

            var requestTime = DateTimeHelper.StampToDateTime(headers["X-CA-TIMESTAMP"].ToString());
            if ((DateTime.Now - requestTime).Minutes >= 1)
                return false;

            var nonce = cache.Get<string>($"nonce-{requestTime}");
            if (!string.IsNullOrEmpty(nonce) && nonce.Equals(headers["X-CA-NONCE"].ToString()))
                return false;
            cache.Set($"nonce-{requestTime}", nonce, TimeSpan.FromMinutes(5));

            return true;
        }

        /// <summary>
        /// 获取请求参数，并根据字母（a~z）排序
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static async Task<SortedDictionary<string, object>> GetRequestData(HttpRequest request)
        {
            var requestData = new SortedDictionary<string, object>();
            var queryString = request.QueryString;
            if (queryString.HasValue)
            {
                var queryArray = queryString.Value.TrimStart('?').Split('&');
                foreach (var item in queryArray)
                {
                    if (!item.Contains('=')) continue;
                    var newArray = item.Split('=');
                    if (string.IsNullOrEmpty(newArray[1]))
                        continue;
                    if (requestData.ContainsKey(newArray[0])) continue;
                    requestData.Add(newArray[0], newArray[1]);
                }
            }
            requestData.Add("key", request.Headers["X-CA-Key"].ToString());
            requestData.Add("nonce", request.Headers["X-CA-NONCE"].ToString());
            requestData.Add("timestamp", request.Headers["X-CA-TIMESTAMP"].ToString());
            var body = await request.Body.GetStringAsync();
            if (string.IsNullOrEmpty(body)) return requestData;

            var dic = body.ToModel<SortedDictionary<string, object>>();
            foreach (var item in dic)
            {
                if (requestData.ContainsKey(item.Key)) continue;
                requestData.Add(item.Key, item.Value);
            }
            return requestData;
        }

        internal static string DicToString(SortedDictionary<string, object> data)
        {
            string str = "";
            foreach (var item in data)
            {
                str += item.Key + item.Value;
            }

            return str;
        }
    }
}
