using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mvc.Core.Filter
{
    public class VaildateFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
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
                var success = false;
                context.Result = new BadRequestObjectResult(new
                {
                    success,
                    message,
                });
            }
        }
    }
}
