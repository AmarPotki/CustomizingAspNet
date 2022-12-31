
// asp.net 3.1
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();
//    }
//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//            });
//}

//asp.net 3.1 withi loging
//public static IHostBuilder CreateHostBuilder(string[] args) =>
//Host.CreateDefaultBuilder(args)
//    .ConfigureWebHostDefaults(webBuilder =>
//    {
//        webBuilder
//            .ConfigureLogging((hostingContext, logging) =>
//            {
//                logging.AddConfiguration(
//                    hostingContext.Configuration.GetSection(
//                        "Logging"));
//                logging.AddConsole();
//                logging.AddDebug();

//            })
//            .UseStartup<Startup>();

//asp.net 6 and above 
//============================================================================================

var builder = WebApplication.CreateBuilder(args);

//logging default
builder.Logging.AddConfiguration(builder.Configuration.
        GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// custom logging
builder.Logging.ClearProviders();
var config = new ColoredConsoleLoggerConfiguration
{
    LogLevel = LogLevel.Information,
    Color = ConsoleColor.Red
};
builder.Logging.AddProvider(new
    ColoredConsoleLoggerProvider(config));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}