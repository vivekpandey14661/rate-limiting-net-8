using Microsoft.Extensions.Options;
using RateLimiter.Application.Options;

namespace RateLimiter.Application.Services;

public class TokenBucketRateLimiter : IRateLimiter
{
    private readonly IRateLimitStore _store;
    private readonly IOptionsMonitor<RateLimitConfig> _options;

    public TokenBucketRateLimiter(
        IRateLimitStore store,
        IOptionsMonitor<RateLimitConfig> options)
    {
        _store = store;
        _options = options;
    }

    public bool IsAllowed(string userId)
    {
        var config = _options.CurrentValue;

        var rule = config.UserRules.ContainsKey(userId)
            ? config.UserRules[userId]
            : config.DefaultRule;

        var bucket = _store.GetOrCreate(
            userId,
            () => new TokenBucket(rule.Capacity, rule.RefillRatePerSecond));

        return bucket.TryConsume();
    }
}
