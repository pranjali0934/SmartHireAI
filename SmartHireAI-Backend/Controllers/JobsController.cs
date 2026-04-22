using Microsoft.AspNetCore.Mvc;
using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Services;

namespace SmartHireAI.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    public async Task<ActionResult<JobDto>> Create([FromBody] CreateJobRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Job title is required.");

        var job = await _jobService.CreateJobAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetAll()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobDto>> GetById(int id)
    {
        var job = await _jobService.GetByIdAsync(id);
        if (job == null)
            return NotFound();
        return Ok(new JobDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            RequiredSkills = job.RequiredSkills,
            CreatedAt = job.CreatedAt
        });
    }
}
