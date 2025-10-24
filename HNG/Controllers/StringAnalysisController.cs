using System.Net;
using System.Threading.Tasks;
using HNG.Services.Interface;
using HNG.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StringAnalysisController : ControllerBase
    {
        private readonly IStringAnalysisService _stringAnalysisService;
        public StringAnalysisController(IStringAnalysisService stringAnalysisService)
        {
            _stringAnalysisService = stringAnalysisService;
        }

        [HttpPost("strings")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public async Task<IActionResult> AnalyzeString([FromBody] AnalysisRequest request)
        {
            try
            {
                var response = await _stringAnalysisService.CreateWord(request.Value);
                if (response is null)
                    return StatusCode((int)HttpStatusCode.Conflict, "String already exists in the system");

                return CreatedAtAction(nameof(GetStringByValue), new { stringValue = request.Value }, response);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("strings/{stringValue}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStringByValue(string stringValue)
        {
            var response = await _stringAnalysisService.GetString(stringValue);
            if (response is null)
                return NotFound();
            return Ok(response);
        }

        [HttpDelete("strings/{stringValue}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStringByValue(string stringValue)
        {
            var response = await _stringAnalysisService.DeleteString(stringValue);
            if (!response)
                return NotFound();
            return NoContent();
        }

        [HttpGet("strings")]
        [ProducesResponseType(typeof(FilteredResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetStrings([FromQuery] FilterRequest filters)
        {
            var response = await _stringAnalysisService.GetStrings(filters);
            return Ok(response);
        }

        [HttpGet("strings/filter-by-natural-language")]
        [ProducesResponseType(typeof(NaturalLanguageFilteredResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetStringsByNaturalLanguage([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return BadRequest("Query parameter cannot be empty.");

                var response = await _stringAnalysisService.GetStringsByNaturalLanguage(query);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}