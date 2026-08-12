using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace LegacyRecords.CaseWareFileUsers.Core.CommandLineOptions
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    [Verb(Constants.ExecutionModes.SampleExecutionMode, HelpText = "Performs some sample task.")]
    public class SampleCommandLineOptions
    {
    }

    // examples of command line options
    // see also: https://github.com/commandlineparser/commandline
    //
    // [Verb(Constants.ExecutionModes.DataMaintenance, HelpText = "Performs data maintenance/cleanup tasks.")]
    // public class DataMaintenanceOptions {}
    //
    // [Verb(Constants.ExecutionModes.AuditFoldersInSyncCopyFileShares, HelpText = "Audits folders in sync copy file shares against the sync copies database.")]
    // public class AuditFoldersInSyncCopyFileSharesOptions
    // {
    //    [Option('d', "delete-unmanaged-items", HelpText = "Delete any items found that are not managed in the sync copies database.", Default = false)]
    //    public bool DeleteUnmanagedItems { get; set; }
    // }
}
