using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddRedis(
        redisConnectionString: "localhost:6379",
        name: "Redis Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "redis" }
    )
    .AddMongoDb(
        mongodbConnectionString: "mongodb://localhost:27017/myDatabase",
        name: "MongoDB Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "mongodb" }
    )
    .AddNpgSql(
        connectionString: "User ID=postgres;Password=12345;Host=localhost;Port=5432;Database=postgres;",
        name: "PostgreSQL",
        healthQuery: "Select 1",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "postgresql", "sql", "db" }
    );

var app = builder.Build();
app.UseHealthChecks("/health",new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
