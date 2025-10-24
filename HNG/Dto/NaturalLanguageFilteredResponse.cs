using System.Text.Json.Serialization;

namespace HNG.Dto
{
    public class NaturalLanguageFilteredResponse
    {
        [JsonPropertyName("data")]
        public List<Response> Data { get; set; } = [];

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("interpreted_query")]
        public InterpretedQuery InterpretedQuery { get; set; } = new();
    }

    public class InterpretedQuery
    {
        [JsonPropertyName("original")]
        public string Original { get; set; } = string.Empty;

        [JsonPropertyName("parsed_filters")]
        public FiltersApplied ParsedFilters { get; set; } = new();
    }
}