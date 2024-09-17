using Aspire.MassTransit.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add HotChocolate services to the container
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGraphQL();

await app.RunAsync();