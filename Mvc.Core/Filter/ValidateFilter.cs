using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Mvc.Core.Attribute;

namespace Mvc.Core.Filter
{
    /// <summary>
    /// 模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ignore = IgnoreVerify(context);
            if (!context.ModelState.IsValid && !ignore)
            {
                var message = "";
                foreach (var item in context.ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        message += error.ErrorMessage + ";";
                    }
                }

                message = string.IsNullOrEmpty(message) ? message : message.TrimEnd(';');
                context.Result = new BadRequestObjectResult(new
                {
                    success = false,
                    message,
                });
            }

            base.OnActionExecuting(context);
        }

        private bool IgnoreVerify(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return true;

            var ignore = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                .Any(p => p.GetType() == typeof(IgnoreVerifyAttribute));

            return ignore;
        }
    }
}
