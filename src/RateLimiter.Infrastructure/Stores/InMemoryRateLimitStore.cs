namespace RateLimiter.Infrastructure.Stores;

using System.Collections.Concurrent;
using RateLimiter.Application.Services;
public class InMemoryRateLimitStore : IRateLimitStore
{
    private readonly ConcurrentDictionary<string, TokenBucket> _store = new();

    public TokenBucket GetOrCreate(string key, Func<TokenBucket> factory)
        => _store.GetOrAdd(key, _ => factory());
}
