    using Microsoft.Extensions.Options;

namespace RateLimiter.Tests;

/// <summary>
/// Simple test implementation of IOptionsMonitor
/// </summary>
public class TestOptionsMonitor<T> : IOptionsMonitor<T>
{
    private readonly T _currentValue;

    public TestOptionsMonitor(T currentValue)
    {
        _currentValue = currentValue;
    }

    public T CurrentValue => _currentValue;

    public T Get(string? name) => _currentValue;

    public IDisposable OnChange(Action<T, string?> listener)
        => new NoOpDisposable();

    private sealed class NoOpDisposable : IDisposable
    {
        public void Dispose() { }
    }
}
