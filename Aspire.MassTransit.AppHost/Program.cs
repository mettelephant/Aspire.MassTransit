var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithOtlpExporter();

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithOtlpExporter();

var username = builder.AddParameter("username", secret: true);
// var password = builder.AddParameter("password", secret: true);
var password = builder.CreateResourceBuilder(
    new ParameterResource("password", _ => "12345pass", secret: true));

var postgresDb = builder.AddPostgres("postgresdb", username, password)
    .WithOtlpExporter()
    .WithPgWeb();

var pgdb = postgresDb.AddDatabase("ApiDb");

var kafka = builder.AddKafka("kafka")
    .WithOtlpExporter()
    .WithKafkaUI();

var pgMigrations = builder.AddProject<Projects.Aspire_MassTransit_Migrator>("dbmigrator")
    .WithReference(pgdb);

const int replicaCount = 3;

var apiService = builder.AddProject<Projects.Aspire_MassTransit_ApiService>("apiservice")
    .WithReference(rabbitMq)
    .WithReference(postgresDb)
    .WithReference(pgdb)
    .WithReference(kafka)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.ApiService")
    .WithReplicas(replicaCount);

builder.AddProject<Projects.Aspire_MassTransit_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReference(kafka)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.Web")
    .WithReplicas(replicaCount);

builder.AddProject<Projects.Aspire_MassTransit_Producer>("producer")
    .WithReference(rabbitMq)
    .WithReference(kafka)
    .WithEnvironment("OTEL_SERVICE_NAME", "Aspire.MassTransit.Producer")
    .WithReplicas(replicaCount);
    
await builder.Build()
    .RunAsync();
