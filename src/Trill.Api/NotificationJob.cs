using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trill.Application.Services;

namespace Trill.Api
{
    public class NotificationJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
                Console.WriteLine("Processing a job...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}