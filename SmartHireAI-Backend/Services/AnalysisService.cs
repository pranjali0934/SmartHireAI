using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Services;

public class AnalysisService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IResumeMatchRepository _resumeMatchRepository;
    private readonly IOpenAIService _openAIService;

    public AnalysisService(
        IResumeRepository resumeRepository,
        IJobRepository jobRepository,
        IResumeMatchRepository resumeMatchRepository,
        IOpenAIService openAIService)
    {
        _resumeRepository = resumeRepository;
        _jobRepository = jobRepository;
        _resumeMatchRepository = resumeMatchRepository;
        _openAIService = openAIService;
    }

    public async Task<MatchAnalysisResponse> AnalyzeMatchAsync(MatchAnalysisRequest request)
    {
        var resume = await _resumeRepository.GetByIdAsync(request.ResumeId)
            ?? throw new KeyNotFoundException($"Resume with ID {request.ResumeId} not found.");
        var job = await _jobRepository.GetByIdAsync(request.JobId)
            ?? throw new KeyNotFoundException($"Job with ID {request.JobId} not found.");

        var resumeText = resume.ExtractedText ?? "No text extracted from resume.";
        var result = await _openAIService.AnalyzeResumeMatchAsync(
            resumeText,
            job.Title,
            job.Description,
            job.RequiredSkills);

        var resumeMatch = await _resumeMatchRepository.AddOrUpdateAsync(new ResumeMatch
        {
            ResumeId = request.ResumeId,
            JobId = request.JobId,
            MatchScore = result.MatchScore,
            MatchingSkills = result.MatchingSkills,
            MissingSkills = result.MissingSkills,
            AIAnalysis = result.AIAnalysis
        });

        return new MatchAnalysisResponse
        {
            ResumeId = resumeMatch.ResumeId,
            JobId = resumeMatch.JobId,
            MatchScore = resumeMatch.MatchScore,
            MatchingSkills = resumeMatch.MatchingSkills,
            MissingSkills = resumeMatch.MissingSkills,
            AIAnalysis = resumeMatch.AIAnalysis
        };
    }

    public async Task<IEnumerable<RankedCandidateDto>> GetRankedCandidatesAsync(int jobId)
    {
        var matches = await _resumeMatchRepository.GetByJobIdOrderedByScoreAsync(jobId);
        return matches.Select(m => new RankedCandidateDto
        {
            CandidateId = m.Resume.CandidateId,
            CandidateName = m.Resume.Candidate.Name,
            Email = m.Resume.Candidate.Email,
            ResumeId = m.ResumeId,
            JobId = m.JobId,
            MatchScore = m.MatchScore,
            MatchingSkills = m.MatchingSkills,
            MissingSkills = m.MissingSkills,
            AIAnalysis = m.AIAnalysis
        });
    }
}
