namespace RateLimiter.Application.Services;

public class TokenBucket
{
    private double _tokens;
    private readonly double _capacity;
    private readonly double _refillRate;
    private DateTime _lastRefill;
    private readonly object _lock = new();

    public TokenBucket(double capacity, double refillRate)
    {
        _capacity = capacity;
        _refillRate = refillRate;
        _tokens = capacity;
        _lastRefill = DateTime.UtcNow;
    }

    public bool TryConsume()
    {
        lock (_lock)
        {
            Refill();

            if (_tokens < 1)
                return false;

            _tokens--;
            return true;
        }
    }

    private void Refill()
    {
        var now = DateTime.UtcNow;
        var elapsed = (now - _lastRefill).TotalSeconds;

        if (elapsed <= 0)
            return;

        _tokens = Math.Min(_capacity, _tokens + elapsed * _refillRate);
        _lastRefill = now;
    }
}
