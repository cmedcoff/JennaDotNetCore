using LoggingConsoleApp1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

var loggerFactory = new LoggerFactory(new[] {new DebugLoggerProvider()});
var logger = loggerFactory.CreateLogger("SomeLoggingCategory");

var o = new NeedsLogging(logger);
o.LogSomething();