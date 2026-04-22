using SmartHireAI.Backend.Entities;

namespace SmartHireAI.Backend.Interfaces;

public interface ICandidateRepository
{
    Task<Candidate?> GetByIdAsync(int id);
    Task<Candidate?> GetByEmailAsync(string email);
    Task<IEnumerable<Candidate>> GetAllAsync();
    Task<Candidate> AddAsync(Candidate candidate);
}
