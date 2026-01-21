using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace EhrLite.TimelineBff.Api.Caching;

public static class DistributedCacheJsonExtensions
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public static async Task<T?> GetJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        CancellationToken ct)
    {
        var bytes = await cache.GetAsync(key, ct);
        if (bytes is null || bytes.Length == 0) return default;
        return JsonSerializer.Deserialize<T>(bytes, JsonOpts);
    }

    public static async Task SetJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        T value,
        TimeSpan ttl,
        CancellationToken ct)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOpts);

        await cache.SetAsync(
            key,
            bytes,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            },
            ct);
    }

    /// <summary>
    /// Cache-aside: returns cached value if present; otherwise uses factory and caches successful results.
    /// </summary>
    public static async Task<T?> GetOrCreateJsonAsync<T>(
        this IDistributedCache cache,
        string key,
        TimeSpan ttl,
        Func<Task<T?>> factory,
        CancellationToken ct,
        Func<T?, bool>? shouldCache = null)
    {
        var cached = await cache.GetJsonAsync<T>(key, ct);
        if (cached is not null) return cached;

        var fresh = await factory();

        // Default: cache only non-null
        if (shouldCache is null)
        {
            if (fresh is null) return fresh;
        }
        else
        {
            if (!shouldCache(fresh)) return fresh;
        }

        await cache.SetJsonAsync(key, fresh!, ttl, ct);
        return fresh;
    }

    public static async Task<IReadOnlyList<T>> GetOrCreateJsonListAsync<T>(
        this IDistributedCache cache,
        string key,
        TimeSpan ttl,
        Func<Task<IReadOnlyList<T>>> factory,
        CancellationToken ct,
        bool cacheEmpty = true)
    {
        var cached = await cache.GetJsonAsync<List<T>>(key, ct);
        if (cached is not null) return cached;

        var fresh = await factory();

        if (!cacheEmpty && fresh.Count == 0)
            return fresh;

        await cache.SetJsonAsync(key, fresh.ToList(), ttl, ct);
        return fresh;
    }
}
