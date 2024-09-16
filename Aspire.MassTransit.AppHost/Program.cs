var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var rabbitMq = builder.AddRabbitMQ("rabbitmq");

var postgresDb = builder.AddPostgres("postgresdb")
    .WithPgWeb();
var pgdb = postgresDb.AddDatabase("ApiDb");

var apiService = builder.AddProject<Projects.Aspire_MassTransit_ApiService>("apiservice")
    .WithReference(rabbitMq)
    .WithReference(postgresDb)
    .WithReference(pgdb)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.ApiService")
    .WithReplicas(3);

builder.AddProject<Projects.Aspire_MassTransit_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.Web")
    .WithReplicas(3);

builder.AddProject<Projects.Aspire_MassTransit_Producer>("producer")
    .WithReference(rabbitMq)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.Producer")
    .WithReplicas(3);

await builder.Build().RunAsync();
