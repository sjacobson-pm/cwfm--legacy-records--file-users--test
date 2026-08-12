using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Core.Options
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class FakeApiOptions
    {
        public string BaseAddress { get; set; } = null!;

        public string Authority { get; set; } = null!;

        public string ClientId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public string AppScope { get; set; } = null!;
    }
}
