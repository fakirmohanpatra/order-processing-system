using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection strings
var postgresConnection = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Postgres connection string is missing");

var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? throw new InvalidOperationException("Redis connection string is missing");

// Health checks (NO RabbitMQ)
builder.Services.AddHealthChecks()
    .AddNpgSql(postgresConnection)
    .AddRedis(redisConnection);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Prometheus
app.UseHttpMetrics();
app.MapMetrics("/metrics");

// Endpoints
app.MapGet("/health", () => Results.Ok("Order Service is healthy!"));
app.MapGet("/orders", () => Results.Ok("Orders!!"));

app.Run();
