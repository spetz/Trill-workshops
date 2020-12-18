using Microsoft.Extensions.DependencyInjection;
using Trill.Application.Services;
using Trill.Core.Repositories;
using Trill.Infrastructure.Caching;

namespace Trill.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IStoryRepository, InMemoryStoryRepository>();
            return services;
        }
    }
}