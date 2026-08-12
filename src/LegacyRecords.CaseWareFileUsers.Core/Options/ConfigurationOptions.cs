using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Core.Options
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class ConfigurationOptions
    {
        public EmailOptions Email { get; set; } = null!;

        public LoggingOptions Logging { get; set; } = null!;

        public FakeApiOptions FakeApi { get; set; } = null!;
    }
}
