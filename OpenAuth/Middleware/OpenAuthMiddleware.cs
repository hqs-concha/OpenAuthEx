using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenAuth.Model;
using OpenAuth.Service;

namespace OpenAuth.Middleware
{
    public class OpenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public OpenAuthMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, OpenAuthOptions options, ITokenService tokenService)
        {
            if (context.Request.Path != "/oauth/connect" && context.Request.Path != "/oauth/refresh-token")
            {
                var token = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token) && token.Trim() != "Bearer")
                {
                    token = token.Replace("Bearer ", "");
                    var temp = await tokenService.CheckToken(token);
                    if (!temp)
                    {
                        context.Response.ContentType = "application/json;charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("token invalid");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
