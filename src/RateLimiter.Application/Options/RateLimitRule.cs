namespace RateLimiter.Application.Options;

public class RateLimitRule
{
    public int Capacity { get; set; }
    public double RefillRatePerSecond { get; set; }
}
    