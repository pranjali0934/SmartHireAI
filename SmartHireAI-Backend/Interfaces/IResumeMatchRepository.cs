using SmartHireAI.Backend.Entities;

namespace SmartHireAI.Backend.Interfaces;

public interface IResumeMatchRepository
{
    Task<ResumeMatch?> GetByIdAsync(int id);
    Task<ResumeMatch?> GetByResumeAndJobAsync(int resumeId, int jobId);
    Task<IEnumerable<ResumeMatch>> GetByJobIdOrderedByScoreAsync(int jobId);
    Task<ResumeMatch> AddOrUpdateAsync(ResumeMatch resumeMatch);
}
