using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace LegacyRecords.CaseWareFileUsers.Models.PlanteMoranApis
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public sealed class JsonPatchReplaceOperation : JsonPatchOperation
    {
        public override string Operation => "replace";

        [JsonProperty("value")]
        public string Value { get; set; } = null!;
    }
}
