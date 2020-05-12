

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Tools.Exception;
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
            var requestData = GetRequestQueryData(request);

            if (request.HasFormContentType)
            {
                GetRequestFormData(request, requestData);
            }
            else
            {
                await GetRequestBodyData(request, requestData);
            }
            
            return requestData;
        }

        /// <summary>
        /// 将Dictionary拼接成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string DicToString(SortedDictionary<string, object> data)
        {
            string str = "";
            foreach (var item in data)
            {
                str += item.Key + item.Value;
            }

            return str;
        }

        #region Utils

        /// <summary>
        /// 获取 QueryString
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static SortedDictionary<string, object> GetRequestQueryData(HttpRequest request)
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
            return requestData;
        }

        /// <summary>
        /// 获取Request Body中的数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        private static async Task GetRequestBodyData(HttpRequest request, SortedDictionary<string, object> requestData)
        {
            try
            {
                // 请求流内容只能读取一次，配置 PostBodyHandlerMiddleware 中间件后可以多次读取，
                // 但Position必须为0才能读取到内容，读取完成之后，从新将 Position 设置为 0
                if (request.Body.Position > 0) request.Body.Position = 0;
                var readerStream = new StreamReader(request.Body);
                var body = await readerStream.ReadToEndAsync();
                request.Body.Position = 0;

                if (string.IsNullOrEmpty(body)) return;

                var dic = body.ToModel<SortedDictionary<string, object>>();
                foreach (var item in dic)
                {
                    if (requestData.ContainsKey(item.Key)) continue;
                    requestData.Add(item.Key, item.Value);
                }
            }
            catch
            {
                throw new CustomException("Incorrect Request Body");
            }
        }

        /// <summary>
        /// 获取 Request Form中的数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestData"></param>
        private static void GetRequestFormData(HttpRequest request, SortedDictionary<string, object> requestData)
        {
            if (request.Form == null) return;
            foreach (var item in request.Form)
            {
                requestData.Add(item.Key, item.Value);
            }
        }

        #endregion
    }
}
