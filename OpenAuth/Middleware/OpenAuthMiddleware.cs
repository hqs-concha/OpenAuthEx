using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OpenAuth.Model;

namespace OpenAuth.Middleware
{
    public class OpenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public OpenAuthMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, OpenAuthOptions options)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("Bearer ", "");
                var url = $"{options.Authority}/oauth2/check?token={token}";
                var result = await GetAsync(url);
                if (!result.ContainsKey("success") || !(bool)result["success"])
                {
                    var jsonStr = JsonConvert.SerializeObject(result);
                    context.Response.ContentType = "application/json;charset=utf-8";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(jsonStr);
                    return;
                }
            }

            await _next(context);
        }

        private static async Task<Dictionary<string, object>> GetAsync(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
        }
    }
}
