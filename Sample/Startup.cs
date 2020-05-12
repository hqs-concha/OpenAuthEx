using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mvc.Core.Extension;
using Mvc.Core.Filter;
using Mvc.Core.Middleware;
using OpenAuth.Extension;

namespace Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DI

            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            #endregion

            #region MVC

            services.AddControllers().AddAntiReplay().AddValidate();

            #endregion

            #region OAuth

            services.AddOpenAuth(options =>
            {
                options.Authority = Configuration["OpenAuth:Authority"];
                options.ClientId = Configuration["OpenAuth:ClientId"];
                options.ClientSecret = Configuration["OpenAuth:ClientSecret"];
                options.ClientName = Configuration["OpenAuth:ClientName"];
            });

            #endregion

            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy("custom", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                });
            });

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                });
                c.OrderActionsBy(o => o.RelativePath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("custom");
            app.UseMiddleware<PostBodyHandlerMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", "V1");
                c.RoutePrefix = "";
            });


            app.UseRouting();

            app.UseOpenAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
