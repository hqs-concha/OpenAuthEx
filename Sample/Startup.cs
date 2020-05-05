using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddControllers(options => options.Filters.Add(typeof(VaildateFilter)))
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
            services.AddOpenAuth(options =>
            {
                options.Authority = Configuration["OpenAuth:Authority"];
                options.ClientId = Configuration["OpenAuth:ClientId"];
                options.ClientSecret = Configuration["OpenAuth:ClientSecret"];
                options.ClientName = Configuration["OpenAuth:ClientName"];
            });

            services.AddCors(options =>
            {
                options.AddPolicy("custom", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("custom");
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseOpenAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
