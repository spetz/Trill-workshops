using Microsoft.Extensions.DependencyInjection;
using Trill.Application.Services;

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