using Microsoft.Extensions.Options;
using RateLimiter.Application.Options;
using StackExchange.Redis;

namespace RateLimiter.Application.Services;


public class ReddisTokenBucketRateLimiter : IRateLimiter
{
    private readonly IDatabase _db;
    private readonly IOptionsMonitor<RateLimitConfig> _options;
    private readonly LuaScript _script;

    public ReddisTokenBucketRateLimiter(
        IConnectionMultiplexer redis,
        IOptionsMonitor<RateLimitConfig> options)
    {
        _db = redis.GetDatabase();
        _options = options;

        _script = LuaScript.Prepare(File.ReadAllText("token_bucket.lua"));
    }

    public bool IsAllowed(string userId)
    {
        
        var rule = ResolveRule(userId);
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var result = (int)_db.ScriptEvaluate(
            _script.OriginalScript,
            new RedisKey[] { $"rate:{userId}" },
            new RedisValue[] { rule.Capacity, rule.RefillRatePerSecond, now });

        return result == 1;
    }

    private RateLimitRule ResolveRule(string userId)
    {
        var cfg = _options.CurrentValue;
        return cfg.UserRules.ContainsKey(userId)
            ? cfg.UserRules[userId]
            : cfg.DefaultRule;
    }
}
