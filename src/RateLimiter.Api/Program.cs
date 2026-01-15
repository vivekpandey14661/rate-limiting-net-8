using RateLimiter.Application.Options;
using RateLimiter.Application.Services;
using RateLimiter.Infrastructure.Stores;
using StackExchange.Redis;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
builder.Services.Configure<RateLimitConfig>(
    builder.Configuration.GetSection("RateLimiting"));

// Configure Rate Limiter
var useRedis = builder.Configuration
    .GetValue<bool>("RateLimiting:UseRedis");

if (useRedis)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]!));
    builder.Services.AddSingleton<IRateLimiter, ReddisTokenBucketRateLimiter>();
}
else
{
    builder.Services.AddSingleton<IRateLimitStore, InMemoryRateLimitStore>();
    builder.Services.AddSingleton<IRateLimiter, TokenBucketRateLimiter>();
}

// Swagger & API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Prometheus middleware
app.UseHttpMetrics();
app.MapMetrics();

// Swagger Middleware (Order matters!)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rate Limiter API v1");
    options.RoutePrefix = ""; 
});

// Map API endpoints
app.MapRateLimiter();

app.Run();
