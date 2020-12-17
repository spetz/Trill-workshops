using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Trill.Api
{
    public class Startup
    {
        // Configure IoC container
        public void ConfigureServices(IServiceCollection services)
        {
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
                endpoints.MapGet("", context => context.Response.WriteAsync("Hello world!"));
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
