namespace CustomizingAspNet.HostedServices.Models
{
    public class WeatherApiResult
    {
        public string City { get; set; }

        public WeatherCondition Weather { get; set; }
    }
}
