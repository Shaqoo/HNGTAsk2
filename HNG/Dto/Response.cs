using System.Text.Json.Serialization;

namespace HNG.Dto
{
    public class Response
    {
        public string Id { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public Property Properties { get; set; } = default!;

        [JsonPropertyName("created_at")]
        public DateTime DateCreated { get; set; }
    }
    public class Property
    {
        public int Length { get; set; }
        [JsonPropertyName("is_palindrome")]
        public bool IsPalindrome { get; set; }
        [JsonPropertyName("unique_characters")]
        public int UniqueCharacters { get; set; }
        [JsonPropertyName("word_count")]
        public int WordCount { get; set; }
        [JsonPropertyName("character_frequency_map")]
        public Dictionary<char, int> CharacterFrequencyMap { get; set; } = [];
        [JsonPropertyName("sha256_hash")]
        public string Sha256Hash { get; set; } = string.Empty;
    }
}