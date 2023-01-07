namespace CustomizingAspNet.HostedServices.BackgroundServices
{
    public class FileProcessingService : BackgroundService
    {
        private readonly FileProcessingChannel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public FileProcessingService(FileProcessingChannel channel,IServiceProvider serviceProvider, ILogger<FileProcessingService> logger)
        {
            _channel = channel;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var fileName in _channel.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation("start processing");
                //process
                using var scope = _serviceProvider.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IResultProcessor>();
                try
                {
                    await using var stream = File.OpenRead(fileName);
                    await processor.ProcessAsync(stream,stoppingToken);
                }
                finally
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}
