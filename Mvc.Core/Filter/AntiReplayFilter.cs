using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mvc.Core.Attribute;
using Mvc.Core.Utils;
using Tools.Utils;

namespace Mvc.Core.Filter
{
    /// <summary>
    /// 防重放 过滤（验证请求）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AntiReplayFilter : ActionFilterAttribute
    {
        private IMemoryCache _cache;
        private ILogger<AntiReplayFilter> _logger;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ignore = IgnoreVerify(context);
            if (!ignore)
            {
                var request = context.HttpContext.Request;
                _cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
                _logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<AntiReplayFilter>)) as ILogger<AntiReplayFilter>;

                var verifyHeader = AntiReplayHelper.VerifyHeader(request, _cache);
                if (!verifyHeader)
                {
                    ResponseHandle(context, "Incorrect request header");
                    return;
                }

                var dataDic = await AntiReplayHelper.GetRequestData(request);
                var dataStr = AntiReplayHelper.DicToString(dataDic);
                var sign = SecretHelper.Md5(dataStr);
                _logger.LogInformation($"request json data:{dataStr}, generate sign:{sign}, request sign:{request.Headers["X-CA-SIGNATURE"].ToString()}");
                if (!sign.Equals(request.Headers["X-CA-SIGNATURE"].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    ResponseHandle(context, "Incorrect signature");
                    return;
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private void ResponseHandle(ActionExecutingContext context, string msg)
        {
            var result = new
            {
                success = false,
                message = msg
            };
            context.Result = new BadRequestObjectResult(result);
        }

        private bool IgnoreVerify(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return true;

            var ignore = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                .Any(p => p.GetType() == typeof(IgnoreAntiReplayAttribute));

            return ignore;
        }
    }
}
