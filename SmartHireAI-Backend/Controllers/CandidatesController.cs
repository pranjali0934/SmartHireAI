using Microsoft.AspNetCore.Mvc;
using SmartHireAI.Backend.Services;

namespace SmartHireAI.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly AnalysisService _analysisService;

    public CandidatesController(AnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpGet("ranked/{jobId:int}")]
    public async Task<ActionResult<IEnumerable<object>>> GetRanked(int jobId)
    {
        var candidates = await _analysisService.GetRankedCandidatesAsync(jobId);
        return Ok(candidates);
    }
}
