using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OpenAuth.Model;
using OpenAuth.Service;

namespace OpenAuth.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, OpenAuthOptions options, ITokenService tokenService)
        {
            var request = context.Request;
            var response = context.Response;
            if (request.Path == "/oauth/connect")
            {
                if (request.Method != HttpMethod.Post.ToString())
                {
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    return;
                } 
                var code = request.Query["code"].ToString();
                if (string.IsNullOrEmpty(code))
                {
                    await ResponseHandle(response, CreateErrorMsg("Code Invalid"));
                    return;
                }
                var token = await tokenService.GetAccessToken(code);
                await ResponseHandle(response, token);
                return;
            }
            else if (request.Path == "/oauth/refresh-token")
            {
                if (request.Method == HttpMethod.Post.ToString())
                {
                    var param = request.Form["refresh_token"].ToString();
                    var token = await tokenService.RefreshToken(param);
                    await ResponseHandle(response, token);
                    return;
                }
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return;
            }

            await _next(context);
        }

        private async Task ResponseHandle(HttpResponse response, string data)
        {
            response.ContentType = "application/json;charset=utf-8";
            response.StatusCode = (int)HttpStatusCode.OK;
            await response.WriteAsync(data);
        }

        private string CreateErrorMsg(string message)
        {
            var result = new
            {
                success = false,
                code = 1,
                message = message
            };
            var data = JsonConvert.SerializeObject(result);
            return data;
        }
    }
}
