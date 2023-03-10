using System.Collections.Concurrent;

public class ColoredConsoleLoggerProvider : ILoggerProvider
{
    private readonly ColoredConsoleLoggerConfiguration
        _config;
    private readonly ConcurrentDictionary<string,
        ColoredConsoleLogger> _loggers =
        new ConcurrentDictionary<string,
            ColoredConsoleLogger>();
    public ColoredConsoleLoggerProvider
        (ColoredConsoleLoggerConfiguration config)
    {
        _config = config;
    }
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name =>
            new ColoredConsoleLogger(name, _config));
    }
    public void Dispose()
    {
        _loggers.Clear();
    }
}