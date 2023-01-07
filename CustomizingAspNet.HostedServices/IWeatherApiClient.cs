using CustomizingAspNet.HostedServices.Models;

namespace CustomizingAspNet.HostedServices
{
    public interface IWeatherApiClient
    {
        Task<WeatherApiResult?> GetWeatherForecastAsync(CancellationToken cancellationToken = default);
    }
}
