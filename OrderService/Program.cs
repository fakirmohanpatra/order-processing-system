using Prometheus;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Configuration / Connection Strings
// -----------------------------
var postgresConnection = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Postgres connection string is missing");

var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Redis connection string is missing");

var rabbitmqConnection = builder.Configuration.GetConnectionString("RabbitMQ")
    ?? throw new InvalidOperationException("RabbitMQ connection string is missing");

// -----------------------------
// Add Services
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------
// RabbitMQ Singleton (7.x)
// -----------------------------
builder.Services.AddSingleton(sp =>
{
    var factory = new ConnectionFactory
    {
        Uri = new Uri(rabbitmqConnection),
        AutomaticRecoveryEnabled = true,
        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
    };

    // ✅ Use async API in a blocking way safely at startup
    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

// -----------------------------
// Health Checks
// -----------------------------
builder.Services.AddHealthChecks()
    .AddNpgSql(postgresConnection, name: "postgres", timeout: TimeSpan.FromSeconds(5))
    .AddRedis(redisConnection, name: "redis", timeout: TimeSpan.FromSeconds(5))
    .AddRabbitMQ(
        sp => sp.GetRequiredService<IConnection>(),
        name: "rabbitmq",
        timeout: TimeSpan.FromSeconds(5)
    );

// -----------------------------
// Build App
// -----------------------------
var app = builder.Build();

// -----------------------------
// Swagger / OpenAPI
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -----------------------------
// Prometheus metrics
// -----------------------------
app.UseRouting();
app.UseHttpMetrics();
app.MapMetrics("/metrics");

// -----------------------------
// Endpoints
// -----------------------------
app.MapGet("/health", () => Results.Ok("Order Service is healthy!"));
app.MapGet("/orders", () => Results.Ok("Orders!!"));

// -----------------------------
// Run
// -----------------------------
app.Run();
