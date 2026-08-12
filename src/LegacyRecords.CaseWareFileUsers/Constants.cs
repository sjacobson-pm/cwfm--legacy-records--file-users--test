using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class Constants
    {
        public const string ApplicationName = "[fill-me-in]";
        public const string UtilityName = "[fill-me-in]";

        public static string ApplicationTitle => $"{ApplicationName} - {UtilityName}";

        public static class ExecutionModes
        {
            public const string SampleExecutionMode = "sample-execution-mode";

            // todo:: this code is just a sample; remove me
            // examples of execution modes
            // public const string CreateSyncCopies = "create-sync-copies";
            // public const string CreateSyncCopyFileShares = "create-sync-copy-file-shares";
            // public const string DataMaintenance = "data-maintenance";
        }
    }
}
