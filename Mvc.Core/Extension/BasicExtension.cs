using System;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Core.Filter;

namespace Mvc.Core.Extension
{
    public  static class BasicExtension
    {
        /// <summary>
        /// 添加防重放 过滤器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddAntiReplay(this IMvcBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddMvcOptions(options => options.Filters.Add<AntiReplayFilter>())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            return builder;
        }

        /// <summary>
        /// 添加模型验证 过滤器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddValidate(this IMvcBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddMvcOptions(options => options.Filters.Add<ValidateFilter>())
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            return builder;
        }
    }
}
