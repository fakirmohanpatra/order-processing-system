using MediatR;
using OrderService.Application.Extensions;
using OrderService.Infrastructure.Extensions;
using OrderService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register application and infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register MediatR handlers from Application assembly
builder.Services.AddMediatR(typeof(OrderService.Application.Commands.CreateOrder.CreateOrderCommand).Assembly);

// OpenTelemetry tracing (Jaeger) via direct TracerProvider builder
Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("order-service"))
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddJaegerExporter(options =>
    {
        options.AgentHost = builder.Configuration.GetValue("Jaeger:Host", "jaeger");
        options.AgentPort = builder.Configuration.GetValue("Jaeger:Port", 6831);
    })
    .Build();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Prometheus metrics endpoints
app.UseHttpMetrics();
app.UseMetricServer();
app.MapControllers();

app.Run();
