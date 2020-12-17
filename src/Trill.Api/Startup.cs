using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trill.Application.Services;
using Trill.Infrastructure;

namespace Trill.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // Configure IoC container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessenger, Messenger>();
            services.AddSingleton<IMessenger, MessengerV2>();
            services.Configure<ApiOptions>(_configuration.GetSection("api"));
        }

        // Configure middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("", context =>
                {
                    var options = context.RequestServices.GetRequiredService<IOptions<ApiOptions>>();
                    var messenger = context.RequestServices.GetRequiredService<IMessenger>();
                    return context.Response.WriteAsync("Hello world!");
                });
                
                endpoints.MapGet("stories/{storyId:guid}", async context =>
                {
                    var storyId = Guid.Parse(context.Request.RouteValues["storyId"].ToString());
                    if (storyId == Guid.Empty)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }
                    
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{}");
                });
                
                endpoints.MapPost("stories", async context =>
                {
                    var storyId = Guid.NewGuid();
                    // Call application layer and save data...
                    context.Response.Headers.Add("Location", $"stories/{storyId}");
                    context.Response.StatusCode = 201;
                });
            });
        }
    }
}
