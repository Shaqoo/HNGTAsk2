using System.Security.Cryptography;
using System.Text;
using HNG.Services.Interface;

namespace HNG.Services.Implementation
{
    public class HelperService : IHelperService
    {
        /// <summary>
        /// Calculates the frequency of each character in a string.
        /// </summary>
        public Task<Dictionary<char, int>> GetCharacterFrequencyMap(string value)
        {
            var frequencyMap = value
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            return Task.FromResult(frequencyMap);
        }

        /// <summary>
        /// Computes the SHA-256 hash of a string.
        /// </summary>
        public Task<string> GetHash(string value)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));

            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return Task.FromResult(builder.ToString());
        }

        /// <summary>
        /// Counts the number of unique characters in a string.
        /// </summary>
        public Task<int> GetUniqueCharacters(string value)
        {
            return Task.FromResult(value.Distinct().Count());
        }

        /// <summary>
        /// Counts the number of words in a string, separated by whitespace.
        /// </summary>
        public Task<int> GetWordCount(string value)
        {
            // Use StringSplitOptions.RemoveEmptyEntries to handle multiple spaces between words.
            string[] words = value.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return Task.FromResult(words.Length);
        }

        /// <summary>
        /// Checks if a string is a palindrome (case-insensitive).
        ///</summary>
        public Task<bool> IsPalindrome(string value)
        {
            string normalized = value.ToLowerInvariant();
            int left = 0;
            int right = normalized.Length - 1;

            while (left < right)
            {
                if (normalized[left] != normalized[right])
                {
                    return Task.FromResult(false);
                }
                left++;
                right--;
            }
            return Task.FromResult(true);
        }
    }
}