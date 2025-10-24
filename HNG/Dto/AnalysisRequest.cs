using System.ComponentModel.DataAnnotations;

namespace HNG.Dto
{
    public class AnalysisRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The 'value' field is required and cannot be empty.")]
        public required string Value { get; set; }
    }
}
