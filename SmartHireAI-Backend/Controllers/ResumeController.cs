using Microsoft.AspNetCore.Mvc;
using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Services;

namespace SmartHireAI.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumeController : ControllerBase
{
    private readonly ResumeService _resumeService;

    public ResumeController(ResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResumeListItemDto>>> GetAll()
    {
        var resumes = await _resumeService.GetAllAsync();
        return Ok(resumes);
    }

    [HttpPost("upload")]
    [RequestSizeLimit(10_485_760)]
    public async Task<ActionResult<ResumeUploadResponse>> Upload(
        IFormFile file,
        [FromForm] string candidateName,
        [FromForm] string candidateEmail)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        if (string.IsNullOrWhiteSpace(candidateName) || string.IsNullOrWhiteSpace(candidateEmail))
            return BadRequest("Candidate name and email are required.");

        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _resumeService.UploadResumeAsync(
                stream, file.FileName, candidateName, candidateEmail);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
