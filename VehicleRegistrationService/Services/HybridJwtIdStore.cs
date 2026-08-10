namespace VehicleRegistrationService.Services;

internal sealed class HybridJwtIdStore(HybridCache  cache, TimeProvider timeProvider) : IJwtIdStore
{
    public async ValueTask RegisterAsync(string jti, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        var lifetime = expiresAt - timeProvider.GetUtcNow();

        if (lifetime <= TimeSpan.Zero)
        {
            return;
        }

        await cache.SetAsync(
            CacheKey(jti),
            value: true,
            new HybridCacheEntryOptions
            {
                Expiration = lifetime,
                LocalCacheExpiration = lifetime,
            },
            cancellationToken: cancellationToken);
    }

    public async ValueTask<bool> IsActiveAsync(
        string jti,
        CancellationToken cancellationToken = default)
    {
        // On miss: return false without writing a negative entry into the cache.
        return await cache.GetOrCreateAsync(
            CacheKey(jti),
            static _ => ValueTask.FromResult(false),
            new HybridCacheEntryOptions
            {
                Flags = HybridCacheEntryFlags.DisableLocalCacheWrite
                        | HybridCacheEntryFlags.DisableDistributedCacheWrite,
            },
            cancellationToken: cancellationToken);
    }

    private static string CacheKey(string jti) => $"jwt:jti:{jti}";
}
