using CustomizingAspNet.HostedServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class WeatherServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForecasting(this IServiceCollection services, IConfiguration config)
    {
      
            services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();
            services.TryAddSingleton<IWeatherForecaster, WeatherForecaster>();
            services.Decorate<IWeatherForecaster, CachedWeatherForecaster>();
   

        return services;
    }
}