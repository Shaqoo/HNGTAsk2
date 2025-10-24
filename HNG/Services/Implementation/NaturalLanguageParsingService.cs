using System.Text.RegularExpressions;
using HNG.Dto;
using HNG.Services.Interface;

namespace HNG.Services.Implementation
{
    public class NaturalLanguageParsingService : INaturalLanguageParsingService
    {
        public FilterRequest ParseQuery(string query)
        {
            var filters = new FilterRequest();
            var lowerQuery = query.ToLowerInvariant();

            if (Regex.IsMatch(lowerQuery, @"\b(palindromic|palindrome)\b"))
            {
                filters.IsPalindrome = true;
            }

            if (Regex.IsMatch(lowerQuery, @"\bsingle word\b"))
            {
                filters.WordCount = 1;
            }

            var longerThanMatch = Regex.Match(lowerQuery, @"longer than (\d+) characters");
            if (longerThanMatch.Success)
            {
                if (filters.MinLength.HasValue) throw new InvalidOperationException("Query parsed but resulted in conflicting filters (e.g., multiple 'longer than' clauses).");
                filters.MinLength = int.Parse(longerThanMatch.Groups[1].Value) + 1;
            }

            var shorterThanMatch = Regex.Match(lowerQuery, @"shorter than (\d+) characters");
            if (shorterThanMatch.Success)
            {
                if (filters.MaxLength.HasValue) throw new InvalidOperationException("Query parsed but resulted in conflicting filters (e.g., multiple 'shorter than' clauses).");

                int parsedMaxLength = int.Parse(shorterThanMatch.Groups[1].Value) - 1;
                if (filters.MinLength.HasValue && filters.MinLength > parsedMaxLength)
                    throw new InvalidOperationException("Query parsed but resulted in conflicting filters (e.g., 'longer than 10' and 'shorter than 5').");
                filters.MaxLength = int.Parse(shorterThanMatch.Groups[1].Value) - 1;
            }

            var containsMatch = Regex.Match(lowerQuery, @"contain(?:ing|s)? the (?:letter|character) '?(.)'?");
            if (containsMatch.Success)
            {
                filters.ContainsCharacter = containsMatch.Groups[1].Value[0];
            }

            if (lowerQuery.Contains("first vowel"))
            {
                filters.ContainsCharacter = 'a';
            }

            if (filters.IsPalindrome == null && filters.WordCount == null && filters.MinLength == null && filters.MaxLength == null && filters.ContainsCharacter == null)
            {
                throw new ArgumentException("Unable to parse natural language query");
            }

            return filters;
        }
    }
}