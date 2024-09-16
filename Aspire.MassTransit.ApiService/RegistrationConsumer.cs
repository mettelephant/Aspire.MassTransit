using System.Diagnostics;
using MassTransit;

namespace Aspire.MassTransit.ApiService;

public class RegistrationConsumer(ILogger<RegistrationConsumer> logger) : IConsumer<CustomerRegistered>
{
    private readonly Random _random = new();
    public async Task Consume(ConsumeContext<CustomerRegistered> context)
    {
        Activity.Current?.AddEvent(new ActivityEvent("CustomerRegistrationRequested"));
        using var activity = Tracing.ActivitySource.StartActivity("Handle Customer Registration");
        logger.LogInformation("Request to register customer recieved at: {time}", DateTimeOffset.Now);
        // Delay for a random time between 15 and 55 seconds
        var delay = _random.Next(15000, 55000);
        await Task.Delay(delay);

        // Send the CustomerSignedUp event
        await context.Publish(new CustomerSignedUp(Guid.NewGuid(),
            context.Message.CustomerName,
            context.Message.RegisteredAt,
            DateTime.Now));
        activity?.AddEvent(new ActivityEvent("Customer Registration Complete"));
    }
}