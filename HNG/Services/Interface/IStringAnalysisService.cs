using HNG.Dto;

namespace HNG.Services.Interface
{
    public interface IStringAnalysisService
    {
        Task<Response?> CreateWord(string word);
        Task<Response?> GetString(string stringValue);
        Task<bool> DeleteString(string stringValue);
        Task<FilteredResponse> GetStrings(FilterRequest filters);
        Task<NaturalLanguageFilteredResponse> GetStringsByNaturalLanguage(string query);
    }
}