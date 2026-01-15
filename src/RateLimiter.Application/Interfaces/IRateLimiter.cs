public interface IRateLimiter
{
    bool IsAllowed(string userId);
}