using System.Diagnostics.CodeAnalysis;

namespace LegacyRecords.CaseWareFileUsers.Models.PlanteMoranApis
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public int TotalPageCount { get; set; }
    }
}
