﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tools.Exception;
using Tools.Utils;

namespace Mvc.Core.Middleware
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var result = "";
                if (e is CustomException)
                {
                    result = CreateErrorMsg(e.Message);
                    await ResponseHandle(context.Response, (int)HttpStatusCode.OK,result);
                }
                else
                {
                    result = CreateErrorMsg("系统错误");
                    logger.LogError($"错误：{e.Message}，详情{e.StackTrace}");
                    await ResponseHandle(context.Response, (int)HttpStatusCode.InternalServerError, result);
                }
            }
        }

        private async Task ResponseHandle(HttpResponse response, int code ,string data)
        {
            response.ContentType = "application/json;charset=utf-8";
            response.StatusCode = code;
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
            var data = JsonHelper.ToJson(result);
            return data;
        }
    }
}
