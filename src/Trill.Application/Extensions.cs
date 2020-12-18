using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Trill.Application.Services;

[assembly:InternalsVisibleTo("Trill.Tests.Unit")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Trill.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IStoryService, StoryService>();
            return services;
        }
    }
}