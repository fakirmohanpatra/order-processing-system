var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var httpClient = new HttpClient();

app.MapGet("/health", () => Results.Ok("API Gateway is healthy!"));

app.Map("/{**path}", async context =>
{
    var requestPath = context.Request.Path.ToString();

    string? targetUri = requestPath.StartsWith("/orders") ? $"http://order-service:80{requestPath}" :
                        requestPath.StartsWith("/payments") ? $"http://payment-service:80{requestPath}" :
                        requestPath.StartsWith("/notifications") ? $"http://notification-service:80{requestPath}" :
                        null;

    if (targetUri == null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Service not found");
        return;
    }

    var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUri + context.Request.QueryString);
    if (context.Request.Body.CanRead && context.Request.ContentLength > 0)
        request.Content = new StreamContent(context.Request.Body);

    var response = await httpClient.SendAsync(request);
    context.Response.StatusCode = (int)response.StatusCode;
    await response.Content.CopyToAsync(context.Response.Body);
});

app.Run();
