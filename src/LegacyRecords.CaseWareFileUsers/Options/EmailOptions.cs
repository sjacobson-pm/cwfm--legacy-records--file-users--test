using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Options
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class EmailOptions
    {
        public string MailHost { get; set; } = null!;

        public string ExecutionErrorsEmailToAddress { get; set; } = null!;

        public string ExecutionErrorsEmailFromAddress { get; set; } = null!;
    }
}
