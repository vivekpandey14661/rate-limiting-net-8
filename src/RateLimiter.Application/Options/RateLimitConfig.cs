namespace RateLimiter.Application.Options;

public class RateLimitConfig
{
    public RateLimitRule DefaultRule { get; set; } = new();
    public Dictionary<string, RateLimitRule> UserRules { get; set; } = new();
}
