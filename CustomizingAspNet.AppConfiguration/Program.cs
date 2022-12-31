using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// use webhost and ConfigureAppConfiguration
builder.WebHost.ConfigureAppConfiguration((builderContext, config) =>
{

   // configure configuration here
    builder.Configuration.AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: true);
});

// simple way 
builder.Configuration.AddJsonFile(
    "appsettings.json",
    optional: false,
    reloadOnChange: true);

//the code snippet shows the encapsulated default configuration to read the 
//appsettings.json files

var env = builder.Environment;
builder.Configuration.SetBasePath(env.ContentRootPath);
builder.Configuration.AddJsonFile(
    "appsettings.json",
    optional: false,
    reloadOnChange: true);
builder.Configuration.AddJsonFile(
    $"appsettings.{env.EnvironmentName}.json",
    optional: true,
    reloadOnChange: true);

//Whenever you customize the application configuration, you should add the configuration 
//via environment variables as a final step, using the AddEnvironmentVariables()
//method.The order of the configuration matters and the configuration providers that 
// you add later on will override the configurations added previously. Be sure that the 
//environment variables always override the configurations that are set via a file

//=====================================================================================

//Using typed configurations

//asp.net 5
//services.Configure<AppSettings>
//    (Configuration.GetSection("AppSettings"));

//asp.net 6
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));


//=========================================================================================

//Configuration using INI files

//var env = builder.Environment;
builder.Configuration.AddIniFile(
    "appsettings.ini",
    optional: false,
    reloadOnChange: true);
builder.Configuration.AddIniFile(
    $"appsettings.{env.EnvironmentName}.ini",
    optional: true,
    reloadOnChange: true);

//=========================================================================================

//Configuration providers

builder.Configuration.Add<MyCustomConfigurationSource>(mycustom =>
{
    var myCustomConfigurationSource = new MyCustomConfigurationSource();
});
builder.Configuration.AddEnvironmentVariables();

// Create extension 
//builder.Configuration.AddMyCustomSource("source", optional:
//    false, reloadOnChange: true);
//============================================

builder.Configuration.AddEnvironmentVariables(prefix: "MyCustomPrefix_");

//use by command line

//set MyCustomPrefix_ = "My key with MyCustomPrefix_ Environment"
//dotnet run

//check by docker, pass variable in docker file



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

app.MapGet("/weatherforecast", (IOptions<AppSettings> _options,IConfiguration _configuration) =>
{
    foreach (var c in _configuration.AsEnumerable())
    {
        Console.WriteLine(c.Key + " = " + c.Value);
    }

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