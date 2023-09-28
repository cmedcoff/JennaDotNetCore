using Microsoft.Extensions.Logging;

namespace LoggingConsole2FromWorkerServiceTemplate;

public class NeedsLogging
{
    private readonly ILogger _logger;

    public NeedsLogging(ILogger logger)
    {
        _logger = logger;
    }

    public void LogSomething()
    {
        _logger.LogInformation("Logging something");
    }
}