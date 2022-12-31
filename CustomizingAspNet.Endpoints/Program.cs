using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

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

//simple endpoint 
app.Map("/map", async context =>
{
    await context.Response.WriteAsync("OK");
});
app.MapGet("/mapget", async context =>
{
    await context.Response.WriteAsync("Map GET");
});
app.MapPost("/mappost", async context =>
{
    await context.Response.WriteAsync("Map POST");
});

app.MapMethods(
    "/mapmethods",
    new[] { "DELETE", "PUT" },
    async context =>
    {
        await context.Response.WriteAsync("Map Methods");

    });


app.MapGet("/mapNum/{Id:int?}", async context=>
{
    var id = context.Request.RouteValues.ContainsKey("Id") ? context.Request.RouteValues["Id"]!.ToString() : "";
    await context.Response.WriteAsync("OK"+id);
}).WithName("mapNum")
    .WithOpenApi();


//==========================================================================
//Creating a more complex endpoint

app.MapMyHealthChecks("/myhealth");



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