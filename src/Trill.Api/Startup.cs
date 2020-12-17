using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Trill.Application;
using Trill.Application.Commands;
using Trill.Application.Services;
using Trill.Infrastructure;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Trill.Api
{
    public class Startup
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // Configure IoC container
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiOptions>(_configuration.GetSection("api"));
            // services.AddHostedService<NotificationJob>();
            services.AddSingleton<ErrorHandlerMiddleware>();
            services.AddApplication();
        }

        // Configure middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Query.TryGetValue("token", out var token) && token == "secret")
                {
                    await ctx.Response.WriteAsync("Secret");
                    return;
                }

                await next();
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("", context =>
                {
                    var options = context.RequestServices.GetRequiredService<IOptions<ApiOptions>>();
                    return context.Response.WriteAsync("Hello world!");
                });
                
                endpoints.MapGet("stories/{storyId:guid}", async context =>
                {
                    var storyId = Guid.Parse(context.Request.RouteValues["storyId"].ToString());
                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    var story = await storyService.GetAsync(storyId);
                    if (story is null)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(story, _jsonSerializerSettings));
                });

                endpoints.MapGet("stories", async context =>
                {
                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    var stories = await storyService.BrowseAsync();
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(stories, _jsonSerializerSettings));
                });
                
                endpoints.MapPost("stories", async context =>
                {
                    var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    var command = JsonConvert.DeserializeObject<SendStory>(json);
                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    await storyService.AddAsync(command);
                    context.Response.Headers.Add("Location", $"stories/{command.Id}");
                    context.Response.StatusCode = 201;
                });
            });
        }
    }
}
