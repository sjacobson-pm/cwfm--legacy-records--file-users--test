using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Options
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class LoggingOptions
    {
        public string ConsoleOutputTemplate { get; set; } = null!;

        public string DebugOutputTemplate { get; set; } = null!;

        public LogLevelOptions LogLevel { get; set; } = null!;

        public LoggingApplicationInsightsOptions ApplicationInsights { get; set; } = null!;
    }
}
