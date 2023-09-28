using LoggingConsoleApp1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

var loggerFactory = new LoggerFactory(new[] {new DebugLoggerProvider()});
var logger = loggerFactory.CreateLogger("MyLogger");
logger.LogInformation("Here's some logging");

var o = new NeedsLogging(logger);
o.LogSomething();