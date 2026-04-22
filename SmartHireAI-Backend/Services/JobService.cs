using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Services;

public class JobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobDto> CreateJobAsync(CreateJobRequest request)
    {
        var job = await _jobRepository.AddAsync(new Job
        {
            Title = request.Title,
            Description = request.Description,
            RequiredSkills = request.RequiredSkills
        });

        return ToDto(job);
    }

    public async Task<IEnumerable<JobDto>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(ToDto);
    }

    public async Task<Job?> GetByIdAsync(int id) =>
        await _jobRepository.GetByIdAsync(id);

    private static JobDto ToDto(Job job) => new()
    {
        Id = job.Id,
        Title = job.Title,
        Description = job.Description,
        RequiredSkills = job.RequiredSkills,
        CreatedAt = job.CreatedAt
    };
}
