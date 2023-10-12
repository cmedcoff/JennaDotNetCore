using CrossDotNetPlatformatBizLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingConsole2FromWorkerServiceTemplate
{
    // adapter pattern
    public class DotNetCoreMyLogger : IMyLogger
    {
        readonly ILogger<DotNetCoreMyLogger> _logger;

        public DotNetCoreMyLogger(ILogger<DotNetCoreMyLogger> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
