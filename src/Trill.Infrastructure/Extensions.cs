using Microsoft.Extensions.DependencyInjection;
using Trill.Application.Services;
using Trill.Core.Repositories;
using Trill.Infrastructure.Caching;
using Trill.Infrastructure.Mongo;

namespace Trill.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IStoryRepository, InMemoryStoryRepository>();
            services.Decorate<IStoryService, StoryServiceCacheDecorator>();
            services.AddMongo();
            
            return services;
        }
    }
}