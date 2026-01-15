
using Moq;
using StackExchange.Redis;
using RateLimiter.Application.Services;
using RateLimiter.Application.Options;
using RateLimiter.Infrastructure.Stores;

namespace RateLimiter.Tests;


public class RateLimiterTests
{
    [Fact]
    public void Should_Block_Request_When_Limit_Exceeded()
    {
        // Arrange
        var store = new InMemoryRateLimitStore();

        var config = new RateLimitConfig
        {
            DefaultRule = new RateLimitRule
            {
                Capacity = 1,
                RefillRatePerSecond = 0
            }
        };


        var limiter = new TokenBucketRateLimiter(
            store,
            new TestOptionsMonitor<RateLimitConfig>(config));

        // Act
        var first = limiter.IsAllowed("user1");
        var second = limiter.IsAllowed("user1");

        // Assert
        Assert.True(first);
        Assert.False(second);
    }
}
