using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aspire.MassTransit.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Random _random = new();
        private readonly Faker _faker;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _faker = new Faker();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = _random.Next(25000, 50000); // Delay between 25 and 50 seconds
                using var activity = Tracing.ActivitySource.StartActivity("Publishing CustomerRegistered Event");
                await Task.Delay(delay, stoppingToken);
                activity?.AddEvent(new ActivityEvent("Customer completed registration"));

                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var customerRegisteredEvent = new CustomerRegistered(
                    CustomerName: _faker.Company.CompanyName(),
                    RegisteredAt: DateTime.Now
                );

                await publishEndpoint.Publish(customerRegisteredEvent, stoppingToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("CustomerRegistered event published at: {time}", DateTimeOffset.Now);
                }
                activity?.AddEvent(new ActivityEvent("Request sent, waiting for more"));
            }
        }
    }
}