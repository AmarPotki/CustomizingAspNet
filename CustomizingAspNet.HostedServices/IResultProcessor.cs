namespace CustomizingAspNet.HostedServices;

public interface IResultProcessor
{
    Task ProcessAsync(Stream stream, CancellationToken cancellationToken = default);
}

public class ResultProcessor : IResultProcessor
{

    private readonly ILogger<ResultProcessor> _logger;

    public ResultProcessor(ILogger<ResultProcessor> logger)
    {

        _logger = logger;
    }

    public async Task ProcessAsync(Stream stream, CancellationToken cancellationToken = default)
    {

        _logger.LogInformation($"Processing ResultProcessor .");

        await Task.Delay(10000);

    }
}