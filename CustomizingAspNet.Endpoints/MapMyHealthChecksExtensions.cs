using System.Diagnostics;
using Microsoft.Extensions.Primitives;

public static class MapMyHealthChecksExtensions
{
    public static IEndpointConventionBuilder
        MapMyHealthChecks(
            this IEndpointRouteBuilder endpoints,
            string pattern = "/myhealth")
    {
        var pipeline = endpoints
            .CreateApplicationBuilder()
            .UseMiddleware<MyHealthChecksMiddleware>()
            
            .Build();
        return endpoints.Map(pattern, pipeline)
            .AddEndpointFilter(async (context, next) =>
            {
                StringValues deviceType;
                context.HttpContext.Request.Headers.TryGetValue("x-device-type", out deviceType);
                if (deviceType != "mobile")
                {
                    return Results.BadRequest();
                }

                var result = await next(context);

                Debug.WriteLine("after");

                return result;
            })
            .WithDisplayName("My custom health checks");
    }
}

public class MyHealthChecksMiddleware
{
    private readonly ILogger<MyHealthChecksMiddleware>
        _logger;
    public MyHealthChecksMiddleware(
        RequestDelegate next,
        ILogger<MyHealthChecksMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        // add some checks here... 
        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("OK");
    }
}
