using Microsoft.AspNetCore.Mvc;
using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Services;

namespace SmartHireAI.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly AnalysisService _analysisService;

    public AnalysisController(AnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost("match")]
    public async Task<ActionResult<MatchAnalysisResponse>> Match([FromBody] MatchAnalysisRequest request)
    {
        try
        {
            var result = await _analysisService.AnalyzeMatchAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
