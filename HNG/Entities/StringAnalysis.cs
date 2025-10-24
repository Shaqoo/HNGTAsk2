namespace HNG.Entities
{
    /// <summary>
    /// Represents the computed properties of an analyzed string.
    /// </summary>
    public class StringAnalysis
    {
        /// <summary>
        /// The SHA-256 hash of the original string, used as a unique identifier.
        /// </summary>
        public required string Id { get; set; }
        public required string Value { get; set; }
        public int Length { get; set; }
        public bool IsPalindrome { get; set; }
        public int UniqueCharacters { get; set; }
        public int WordCount { get; set; }
        public required Dictionary<char, int> CharacterFrequencyMap { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
