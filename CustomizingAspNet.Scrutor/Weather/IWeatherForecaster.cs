
namespace CustomizingAspNet.Scrutor.Weather
{
	public interface IWeatherForecaster
	{
		Task<WeatherResult> GetCurrentWeatherAsync(string city);
	}
}