using System.Diagnostics;
using Bogus;
using MassTransit;

namespace Aspire.MassTransit.ApiService;

public class CustomerSignedUpConsumer(ILogger<CustomerSignedUpConsumer> logger) : IConsumer<CustomerSignedUp>
{
    private readonly Random _random = new();
    private readonly Faker _faker = new();
    public async Task Consume(ConsumeContext<CustomerSignedUp> context)
    {
        Activity.Current?.AddEvent(new ActivityEvent("CustomerCompletelySignedUp"));
        using var activity = Tracing.ActivitySource.StartActivity("Kick off Customer Provisioning");
        logger.LogInformation("Provisoning Customer DB: {time}", DateTimeOffset.Now);
        // Delay for a random time between 15 and 55 seconds
        var delay = _random.Next(15000, 55000);
        await Task.Delay(delay);

        // Send the CustomerSignedUp event
        await context.Publish(new CustomerProvisioned(context.Message.CustomerId,
            context.Message.CustomerName,
            _faker.Database.Random.String2(15),
            DateTime.Now));
        activity?.AddEvent(new ActivityEvent("Customer Registration Complete"));
    }
}