using Aspire.MassTransit.Producer;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var messaging = builder.Configuration.GetConnectionString("rabbitmq");
        cfg.Host(messaging);

        // Configure endpoints for the consumers
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();