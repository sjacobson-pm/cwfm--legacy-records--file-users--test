using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Core.Options
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class LoggingApplicationInsightsOptions
    {
        public string ConnectionString { get; set; } = null!;
    }
}
