using System.Text.Json.Serialization;
using CustomizingAspNet.HostedServices.Models;
using Newtonsoft.Json;

namespace CustomizingAspNet.HostedServices
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;

        public WeatherApiClient(HttpClient httpClient, ILogger<WeatherApiClient> logger)
        {
          

            httpClient.BaseAddress = new Uri("http://localhost:5001");

            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<WeatherApiResult?> GetWeatherForecastAsync(CancellationToken cancellationToken = default)
        {
            const string path = "api/currentWeather/Brighton";

            try
            {
                var response = await _httpClient.GetAsync(path, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                return JsonConvert.DeserializeObject<WeatherApiResult>(content);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Failed to get weather data from API");
            }

            return null;
        }
    }
}
