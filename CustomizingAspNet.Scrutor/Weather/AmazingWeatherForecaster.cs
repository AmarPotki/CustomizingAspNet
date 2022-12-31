namespace CustomizingAspNet.Scrutor.Weather
{
	public class AmazingWeatherForecaster : IWeatherForecaster
	{
		public Task<WeatherResult> GetCurrentWeatherAsync(string city)
		{
			// DO SOMETHING AMAZING HERE!!!

			return Task.FromResult(new WeatherResult()
			{
				City = city,
				Weather = new WeatherCondition
				{
					Summary = "Sun"
				}
			});
		}
	}
}
