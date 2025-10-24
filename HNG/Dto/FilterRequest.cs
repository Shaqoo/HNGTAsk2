using Microsoft.AspNetCore.Mvc;

namespace HNG.Dto
{
    public class FilterRequest
    {
        [FromQuery(Name = "is_palindrome")] public bool? IsPalindrome { get; set; }
        [FromQuery(Name = "min_length")] public int? MinLength { get; set; }
        [FromQuery(Name = "max_length")] public int? MaxLength { get; set; }
        [FromQuery(Name = "word_count")] public int? WordCount { get; set; }
        [FromQuery(Name = "contains_character")] public char? ContainsCharacter { get; set; }
    }
}