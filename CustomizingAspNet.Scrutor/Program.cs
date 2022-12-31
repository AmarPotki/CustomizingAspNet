using CustomizingAspNet.Scrutor.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherForecaster, RandomWeatherForecaster>();
builder.Services.Decorate<IWeatherForecaster, CachedWeatherForecaster>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/weather/{city}", async (string city, IWeatherForecaster forecaster) =>
    {
        var forecast = await forecaster.GetCurrentWeatherAsync(city);
        return forecast.Weather;
    })
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
