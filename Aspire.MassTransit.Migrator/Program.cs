using Aspire.MassTransit.ApiService;
using Aspire.MassTransit.Migrator;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.AddNpgsqlDbContext<MyDbContext>("ApiDb");
builder.EnrichNpgsqlDbContext<MyDbContext>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();
host.Run();