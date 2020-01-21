using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenAuth.Middleware;
using OpenAuth.Model;
using OpenAuth.Service;

namespace OpenAuth.Extension
{
    public static class OpenAuthExtension
    {
        public static void AddOpenAuth(this IServiceCollection services, Action<OpenAuthOptions> action)
        {
            var option = new OpenAuthOptions();
            action.Invoke(option);

            if (string.IsNullOrEmpty(option.ClientId) || string.IsNullOrEmpty(option.ClientSecret))
                throw new ArgumentNullException($"{nameof(option.ClientId)} or {nameof(option.ClientSecret)} must not be null");

            services.AddScoped(o => option);
            services.AddScoped<ITokenService, TokenService>();
            services.AddAuthentication(options =>
                {
                    //认证middleware配置
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token颁发机构
                        ValidIssuer = "OpenAuth",
                        //颁发给谁
                        ValidAudience = option.ClientName,
                        //这里的key要进行加密
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.ClientSecret)),
                        //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                    };
                    config.SaveToken = true;
                });
        }

        public static void UseOpenAuth(this IApplicationBuilder app)
        {
            app.UseMiddleware<TokenMiddleware>();
            app.UseMiddleware<OpenAuthMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
