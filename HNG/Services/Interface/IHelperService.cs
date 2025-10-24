namespace HNG.Services.Interface
{
    public interface IHelperService
    {
        Task<string> GetHash(string value);
        Task<bool> IsPalindrome(string value);
        Task<int> GetUniqueCharacters(string value);
        Task<int> GetWordCount(string value);
        Task<Dictionary<char, int>> GetCharacterFrequencyMap(string value);
    }
}