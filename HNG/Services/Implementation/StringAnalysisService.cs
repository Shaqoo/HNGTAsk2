using HNG.Dto;
using HNG.Entities;
using HNG.Services.Interface;

namespace HNG.Services.Implementation
{
    public class StringAnalysisService : IStringAnalysisService
    {
        private readonly IHelperService _helperService;
        private readonly INaturalLanguageParsingService _naturalLanguageParsingService;

        public StringAnalysisService(IHelperService helperService, INaturalLanguageParsingService naturalLanguageParsingService)
        {
            _helperService = helperService;
            _naturalLanguageParsingService = naturalLanguageParsingService;
        }
        public async Task<Response?> CreateWord(string word)
        {
            if (word.Length == 0)
            {
                throw new ArgumentNullException(nameof(word));
            }
            if(DataStorage.DataStorage.StringAnalyses.Any(x => x.Value == word))
                return null;

            var count = await _helperService.GetWordCount(word);
            var isPalindrome = await _helperService.IsPalindrome(word);
            var uniqueCharacters = await _helperService.GetUniqueCharacters(word);
            var characterFrequencyMap = await _helperService.GetCharacterFrequencyMap(word);
            var hash = await _helperService.GetHash(word);

            DataStorage.DataStorage.StringAnalyses.Add(new StringAnalysis
            {
                Id = hash,
                Value = word,
                Length = count,
                IsPalindrome = isPalindrome,
                UniqueCharacters = uniqueCharacters,
                WordCount = count,
                CharacterFrequencyMap = characterFrequencyMap,
                DateCreated = DateTime.UtcNow
            });


            var response = new Response();
            response.Id = hash;
            response.Value = word;
            response.DateCreated = DateTime.UtcNow;

            var property = new Property();
            property.Length = count;
            property.IsPalindrome = isPalindrome;
            property.UniqueCharacters = uniqueCharacters;
            property.WordCount = count;
            property.CharacterFrequencyMap = characterFrequencyMap;
            property.Sha256Hash = hash;
            response.Properties = property;

            return response;
        }

        public Task<bool> DeleteString(string stringValue)
        {
            if (stringValue.Length == 0)
            {
                throw new ArgumentNullException(nameof(stringValue));
            }
            if (DataStorage.DataStorage.StringAnalyses.Any(x => x.Value == stringValue))
            {
                var word = DataStorage.DataStorage.StringAnalyses.FirstOrDefault(x => x.Value == stringValue);
                if (word == null)
                {
                    return Task.FromResult(false);
                }
                DataStorage.DataStorage.StringAnalyses.Remove(word);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Response?> GetString(string stringValue)
        {
            var word = DataStorage.DataStorage.StringAnalyses.FirstOrDefault(x => x.Value == stringValue);
            if (word == null)
            {
                return Task.FromResult(new Response() ?? null);
            }
            var response = new Response();
            response.Id = word.Id;
            response.Value = word.Value;
            response.DateCreated = word.DateCreated;
            var property = new Property();
            property.Length = word.Length;
            property.IsPalindrome = word.IsPalindrome;
            property.UniqueCharacters = word.UniqueCharacters;
            property.WordCount = word.WordCount;
            property.CharacterFrequencyMap = word.CharacterFrequencyMap;
            property.Sha256Hash = word.Id;
            response.Properties = property;
            return Task.FromResult(response ?? null);
        }

        public Task<FilteredResponse> GetStrings(FilterRequest filters)
        {
            IEnumerable<StringAnalysis> query = DataStorage.DataStorage.StringAnalyses;

            if (filters.IsPalindrome.HasValue)
                query = query.Where(s => s.IsPalindrome == filters.IsPalindrome.Value);

            if (filters.MinLength.HasValue)
                query = query.Where(s => s.Length >= filters.MinLength.Value);

            if (filters.MaxLength.HasValue)
                query = query.Where(s => s.Length <= filters.MaxLength.Value);

            if (filters.WordCount.HasValue)
                query = query.Where(s => s.WordCount == filters.WordCount.Value);

            if (filters.ContainsCharacter.HasValue)
                query = query.Where(s => s.Value.Contains(filters.ContainsCharacter.Value, StringComparison.OrdinalIgnoreCase));

            var results = query.ToList();

            var data = results.Select(word => new Response
            {
                Id = word.Id,
                Value = word.Value,
                DateCreated = word.DateCreated,
                Properties = new Property
                {
                    Length = word.Length,
                    IsPalindrome = word.IsPalindrome,
                    UniqueCharacters = word.UniqueCharacters,
                    WordCount = word.WordCount,
                    CharacterFrequencyMap = word.CharacterFrequencyMap,
                    Sha256Hash = word.Id
                }
            }).ToList();

            var response = new FilteredResponse
            {
                Data = data,
                Count = data.Count,
                FiltersApplied = new FiltersApplied
                {
                    IsPalindrome = filters.IsPalindrome,
                    MinLength = filters.MinLength,
                    MaxLength = filters.MaxLength,
                    WordCount = filters.WordCount,
                    ContainsCharacter = filters.ContainsCharacter
                }
            };
            return Task.FromResult(response);
        }

        public async Task<NaturalLanguageFilteredResponse> GetStringsByNaturalLanguage(string query)
        {
            var parsedFilters = _naturalLanguageParsingService.ParseQuery(query);

            var filteredResult = await GetStrings(parsedFilters);

            var response = new NaturalLanguageFilteredResponse
            {
                Data = filteredResult.Data,
                Count = filteredResult.Count,
                InterpretedQuery = new InterpretedQuery
                {
                    Original = query,
                    ParsedFilters = new FiltersApplied
                    {
                        IsPalindrome = parsedFilters.IsPalindrome,
                        MinLength = parsedFilters.MinLength,
                        MaxLength = parsedFilters.MaxLength,
                        WordCount = parsedFilters.WordCount,
                        ContainsCharacter = parsedFilters.ContainsCharacter
                    }
                }
            };
            return response;
        }
    }
}