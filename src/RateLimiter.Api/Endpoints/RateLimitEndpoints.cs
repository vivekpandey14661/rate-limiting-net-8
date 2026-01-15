
public static class RateLimitEndpoints
{
    public static void MapRateLimiter(this WebApplication app)
    {
        app.MapPost("/check", (
            RateLimitRequest request,
            IRateLimiter limiter) =>
        {
            return limiter.IsAllowed(request.UserId)
                ? Results.Ok()
                : Results.StatusCode(429);
        });
    }
}
