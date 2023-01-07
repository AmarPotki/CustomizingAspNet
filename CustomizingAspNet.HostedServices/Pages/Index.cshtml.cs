using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomizingAspNet.HostedServices.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeatherForecaster _weatherForecaster;

        public IndexModel(ILogger<IndexModel> logger, IWeatherForecaster weatherForecaster)
        {
            _logger = logger;
            _weatherForecaster = weatherForecaster;
        }
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; } = true;

        public async Task OnGet()
        {
            if (ShowWeatherForecast)
            {
                ForecastSectionTitle ="How's the weather?" ;

                var currentWeather = await _weatherForecaster.GetCurrentWeatherAsync();

                if (currentWeather != null)
                {
                    switch (currentWeather.Description)
                    {
                        case "Sun":
                            WeatherDescription = "It's sunny right now. A great day for tennis!";
                            break;

                        case "Cloud":
                            WeatherDescription = "It's cloudy at the moment and the outdoor courts are in use.";
                            break;

                        case "Rain":
                            WeatherDescription = "We're sorry but it's raining here. No outdoor courts in use.";
                            break;

                        case "Snow":
                            WeatherDescription = "It's snowing!! Outdoor courts will remain closed until the snow has cleared.";
                            break;
                    }
                }
            }
        }
    }
}