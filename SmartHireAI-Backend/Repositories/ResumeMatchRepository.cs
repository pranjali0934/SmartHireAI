using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Data;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Repositories;

public class ResumeMatchRepository : IResumeMatchRepository
{
    private readonly ApplicationDbContext _context;

    public ResumeMatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResumeMatch?> GetByIdAsync(int id) =>
        await _context.ResumeMatches
            .Include(rm => rm.Resume).ThenInclude(r => r!.Candidate)
            .Include(rm => rm.Job)
            .FirstOrDefaultAsync(rm => rm.Id == id);

    public async Task<ResumeMatch?> GetByResumeAndJobAsync(int resumeId, int jobId) =>
        await _context.ResumeMatches
            .FirstOrDefaultAsync(rm => rm.ResumeId == resumeId && rm.JobId == jobId);

    public async Task<IEnumerable<ResumeMatch>> GetByJobIdOrderedByScoreAsync(int jobId) =>
        await _context.ResumeMatches
            .Include(rm => rm.Resume).ThenInclude(r => r!.Candidate)
            .Include(rm => rm.Job)
            .Where(rm => rm.JobId == jobId)
            .OrderByDescending(rm => rm.MatchScore)
            .ToListAsync();

    public async Task<ResumeMatch> AddOrUpdateAsync(ResumeMatch resumeMatch)
    {
        var existing = await GetByResumeAndJobAsync(resumeMatch.ResumeId, resumeMatch.JobId);
        if (existing != null)
        {
            existing.MatchScore = resumeMatch.MatchScore;
            existing.MatchingSkills = resumeMatch.MatchingSkills;
            existing.MissingSkills = resumeMatch.MissingSkills;
            existing.AIAnalysis = resumeMatch.AIAnalysis;
            _context.ResumeMatches.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        _context.ResumeMatches.Add(resumeMatch);
        await _context.SaveChangesAsync();
        return resumeMatch;
    }
}
