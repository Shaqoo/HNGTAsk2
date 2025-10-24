using System.Text.Json.Serialization;

namespace HNG.Dto
{
    public class FilteredResponse
    {
        [JsonPropertyName("data")]
        public List<Response> Data { get; set; } = [];

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("filters_applied")]
        public object FiltersApplied { get; set; } = new();
    }

    public class FiltersApplied
    {
        [JsonPropertyName("is_palindrome")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsPalindrome { get; set; }
        [JsonPropertyName("min_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MinLength { get; set; }
        [JsonPropertyName("max_length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxLength { get; set; }
        [JsonPropertyName("word_count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WordCount { get; set; }
        [JsonPropertyName("contains_character")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public char? ContainsCharacter { get; set; }
    }
}