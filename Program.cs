using StarWarsServer.Hubs;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;

        // ✅ Re-enable reflection-based serialization manually
        options.PayloadSerializerOptions.TypeInfoResolverChain.Add(
            new DefaultJsonTypeInfoResolver());
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7110") // your Blazor dev port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Trace);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Trace);
builder.Logging.AddFilter("Microsoft.AspNetCore.Routing", LogLevel.Debug);

var app = builder.Build();

app.UseCors();
app.Use(async (context, next) =>
{
    Console.WriteLine($"➡️ {context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapHub<GameHub>("/gamehub");

AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
{
    var ex = eventArgs.Exception;
    if (ex.Source?.Contains("SignalR") == true)
        Console.WriteLine($"⚠️ FirstChanceException: {ex.GetType().Name} - {ex.Message}");
};


app.Run();
