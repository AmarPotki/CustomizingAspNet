using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseKestrel((host,options) =>
{
    var filename = host.Configuration.GetValue(
        "AppSettings:certfilename", "");
    var password = host.Configuration.GetValue(
        "AppSettings:certpassword", "");
    options.Listen(IPAddress.Loopback, 5000);
    options.Listen(IPAddress.Loopback, 5001,
        listenOptions =>
        {
            listenOptions.UseHttps(filename,
                password);
        });
});

//.net cli
//dotnet user-secrets init
//dotnet user-secrets set "AppSettings:certfilename" "certificate.pfx"
//dotnet user-secrets set "AppSettings:certpassword" "topsecret"

//this also set environment variables
//SET APPSETTINGS_CERTFILENAME = certificate.pfx
//SET APPSETTINGS_CERTPASSWORD=topsecret

//=======================================================
//UseHttpSys

//builder.WebHost.UseHttpSys(options =>
//{
//    // ...
//});

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

app.MapGet("/weatherforecast", (IConfiguration _configuration) =>
    {
        var env = _configuration.AsEnumerable();
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
