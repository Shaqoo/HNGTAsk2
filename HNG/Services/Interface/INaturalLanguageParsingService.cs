using HNG.Dto;

namespace HNG.Services.Interface
{
    public interface INaturalLanguageParsingService
    {
        FilterRequest ParseQuery(string query);
    }
}