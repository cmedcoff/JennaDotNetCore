using Microsoft.Extensions.Logging;

namespace LoggingConsole2FromWorkerServiceTemplate;

public class NeedsGenericLogger
{
    private readonly ILogger<NeedsGenericLogger> _logger;

    public NeedsGenericLogger(ILogger<NeedsGenericLogger> logger)
    {
        _logger = logger;
    }

    public void LogSomething()
    {
        _logger.LogInformation("Logging something");
    }
}