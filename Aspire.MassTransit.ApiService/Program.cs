using Aspire.MassTransit.ApiService;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<CustomerSignedUpConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var messaging = builder.Configuration.GetConnectionString("rabbitmq");
        cfg.Host(messaging);

        // Configure endpoints for the consumers
        cfg.ConfigureEndpoints(context);
    });
});

builder.AddNpgsqlDbContext<MyDbContext>("ApiDb");
builder.EnrichNpgsqlDbContext<MyDbContext>();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapDefaultEndpoints();

await app.RunAsync();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
