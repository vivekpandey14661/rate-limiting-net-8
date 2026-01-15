using StackExchange.Redis;
using System.Text.Json;
using RateLimiter.Application.Services;

namespace RateLimiter.Infrastructure.Stores;

public class RedisRateLimitStore : IRateLimitStore
{
    private readonly IDatabase _db;

    public RedisRateLimitStore(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public TokenBucket GetOrCreate(string key, Func<TokenBucket> factory)
    {
        var value = _db.StringGet(key);

        if (!value.HasValue)
        {
            var bucket = factory();
            Save(key, bucket);
            return bucket;
        }

        var bucketFromRedis = JsonSerializer.Deserialize<TokenBucket>(value!);

        if (bucketFromRedis is null)
        {
            var bucket = factory();
            Save(key, bucket);
            return bucket;
        }

        return bucketFromRedis;
    }

    private void Save(string key, TokenBucket bucket)
    {
        var json = JsonSerializer.Serialize(bucket);
        _db.StringSet(key, json);
    }
}
