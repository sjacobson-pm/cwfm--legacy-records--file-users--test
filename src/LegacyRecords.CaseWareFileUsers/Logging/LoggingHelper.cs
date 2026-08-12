using System;
using Serilog.Core;
using Serilog.Events;

namespace LegacyRecords.CaseWareFileUsers.Logging
{
    public static class LoggingHelper
    {
        public static LogEventLevel GetLogEventLevel(string logLevel)
        {
            return Enum.TryParse(logLevel, true, out LogEventLevel logEventLevel) ? logEventLevel : LogEventLevel.Debug;
        }

        public static LoggingLevelSwitch GetLoggingLevelSwitch(string logLevel)
        {
            var loggingLevelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = GetLogEventLevel(string.Equals(logLevel, "Error", StringComparison.OrdinalIgnoreCase) ? "Warning" : logLevel)
            };

            return loggingLevelSwitch;
        }
    }
}
