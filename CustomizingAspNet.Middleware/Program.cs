using CustomizingAspNet.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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

app.Map("/map1", app1 =>
{
    // some more Middleware
    app1.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 1");
    });

});
app.Map("/map2", app2 =>
{
// some more Middleware
    app2.Run(async context => { await context.Response.WriteAsync("Map Test 2"); });
});

//some more Middleware


//==============================================================================

//Branching the pipeline with MapWhen()

app.MapWhen(
    context =>
        context.Request.Query.ContainsKey("branch"),
    app1 =>
    {
        // some more Middleware
        app1.Run(async context =>
        {
            await context.Response.WriteAsync(
                "MapBranch Test");
        });
    });
// some more Middleware
app.Run(async context =>
{
    await context.Response.WriteAsync(
        "Hello from non-Map delegate.");
});


//=====================================================================================

//Creating conditions with middleware

app.UseHealthChecks("/map3");

//======================================================================================

//Prior to.NET 6.0, you would map custom endpoints on the endpoints
//object inside the lambda that gets passed to the UseEndpoints method in
//the Startup.cs file. With.NET 6.0 and the new minimal API approach, 
//the mapping gets done on the app object in the Program.cs file.

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/", async context =>
//    {
//        await context.Response.WriteAsync("Hello World!");
//    });
//});

// Razor Pages:
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();

});
app.MapRazorPages();

// Areas for MVC and web API:
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("","");

});
app.MapControllerRoute("", "");

// Health checks
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHealthChecks("");

});
app.MapHealthChecks("");

//========================================================
//Rewriting terminating middleware to meet the current standards

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Hello World!");
    endpoints.MapAppStatus("/status", "Status");
});

app.MapAppStatus("/status", "Status");



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


public static class UseHealthChecksCoreExtension
{
    private static void UseHealthChecksCore(IApplicationBuilder
        app, PathString path, int? port, object[] args)
    {
        if (port == null)
        {
            app.Map(path,
                b =>
                    b.UseMiddleware<HealthCheckMiddleware>(args));
        }
        else
        {
            app.MapWhen(
                c => c.Connection.LocalPort == port,
                b0 => b0.Map(path,
                    b1 =>
                        b1.UseMiddleware<HealthCheckMiddleware>(args)
                )
            );
        }

        ;
    }
}
