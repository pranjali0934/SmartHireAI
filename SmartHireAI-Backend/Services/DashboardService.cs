using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Data;

namespace SmartHireAI.Backend.Services;

public class DashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly IResumeMatchRepository _resumeMatchRepository;

    public DashboardService(ApplicationDbContext context, IResumeMatchRepository resumeMatchRepository)
    {
        _context = context;
        _resumeMatchRepository = resumeMatchRepository;
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var totalResumes = await _context.Resumes.CountAsync();
        var totalJobs = await _context.Jobs.CountAsync();
        var totalCandidates = await _context.Candidates.CountAsync();

        var topMatches = await _context.ResumeMatches
            .Include(rm => rm.Resume).ThenInclude(r => r!.Candidate)
            .OrderByDescending(rm => rm.MatchScore)
            .Take(10)
            .ToListAsync();

        var topCandidates = topMatches.Select(m => new RankedCandidateDto
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
        }).ToList();

        return new DashboardStatsDto
        {
            TotalResumes = totalResumes,
            TotalJobs = totalJobs,
            TotalCandidates = totalCandidates,
            TopCandidates = topCandidates
        };
    }
}
