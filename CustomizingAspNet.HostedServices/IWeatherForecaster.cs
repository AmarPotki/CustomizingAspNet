using CustomizingAspNet.HostedServices.Models;

namespace CustomizingAspNet.HostedServices;

public interface IWeatherForecaster
{
    Task<CurrentWeatherResult> GetCurrentWeatherAsync();

    bool ForecastEnabled { get; }
}