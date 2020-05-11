using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mvc.Core.Middleware
{
    /// <summary>
    /// 开启 重复读取 请求流内容
    /// </summary>
    public class PostBodyHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public PostBodyHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            await _next(context);
        }
    }
}
