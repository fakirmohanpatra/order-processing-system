var builder = WebApplication.CreateBuilder(args);

// Swagger (optional, useful for testing API Gateway itself)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// For simplicity, disable HTTPS redirection (Nginx handles HTTPS)
app.UseHttpsRedirection();

// Create an HttpClient to forward requests
var httpClient = new HttpClient();

// Order Service Proxy
app.Map("/{**path}", async context =>
{
    var requestPath = context.Request.Path.ToString();
    var targetUri = "";

    if (requestPath.StartsWith("/orders"))
        targetUri = $"http://order-service:80{requestPath["/orders".Length..]}{context.Request.QueryString}";
    else if (requestPath.StartsWith("/payments"))
        targetUri = $"http://payment-service:80{requestPath["/payments".Length..]}{context.Request.QueryString}";
    else if (requestPath.StartsWith("/notifications"))
        targetUri = $"http://notification-service:80{requestPath["/notifications".Length..]}{context.Request.QueryString}";
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Service not found");
        return;
    }

    var response = await httpClient.SendAsync(new HttpRequestMessage(
        new HttpMethod(context.Request.Method),
        targetUri
    ));

    context.Response.StatusCode = (int)response.StatusCode;
    foreach (var header in response.Headers)
        context.Response.Headers[header.Key] = header.Value.ToArray();

    await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
});

// Optional: Root endpoint to test API Gateway
app.MapGet("/", () => "API Gateway is running...");

app.Run();
