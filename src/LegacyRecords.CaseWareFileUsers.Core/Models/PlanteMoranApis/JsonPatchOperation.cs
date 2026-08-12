using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace LegacyRecords.CaseWareFileUsers.Core.Models.PlanteMoranApis
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public abstract class JsonPatchOperation
    {
        [JsonProperty("op")]
        public abstract string Operation { get; }

        [JsonProperty("path")]
        public string Path { get; set; } = null!;
    }
}
