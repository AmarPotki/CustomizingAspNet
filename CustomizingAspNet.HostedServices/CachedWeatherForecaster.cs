using CustomizingAspNet.HostedServices.Caching;
using CustomizingAspNet.HostedServices.Models;
using Microsoft.Extensions.Options;

namespace CustomizingAspNet.HostedServices;

public class CachedWeatherForecaster : IWeatherForecaster
{
    private readonly IWeatherForecaster _weatherForecaster;
    private readonly IDistributedCache<CurrentWeatherResult> _cache;
    private readonly int _minsToCache;

    public bool ForecastEnabled => _weatherForecaster.ForecastEnabled;

    public CachedWeatherForecaster(IWeatherForecaster weatherForecaster,
        IDistributedCache<CurrentWeatherResult> cache)
    {
        _weatherForecaster = weatherForecaster;
        _cache = cache;
        _minsToCache = 30;
    }

    public async Task<CurrentWeatherResult> GetCurrentWeatherAsync()
    {
        var cacheKey = $"current_weather_{DateTime.UtcNow:yyyy_MM_dd}";

        var (isCached, forecast) = await _cache.TryGetValueAsync(cacheKey);

        if (isCached)
            return forecast;

        var result = await _weatherForecaster.GetCurrentWeatherAsync();

        if (result != null)
            await _cache.SetAsync(cacheKey, result, _minsToCache);

        return result;
    }
}