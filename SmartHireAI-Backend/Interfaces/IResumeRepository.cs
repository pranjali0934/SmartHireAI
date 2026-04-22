using SmartHireAI.Backend.Entities;

namespace SmartHireAI.Backend.Interfaces;

public interface IResumeRepository
{
    Task<Resume?> GetByIdAsync(int id);
    Task<IEnumerable<Resume>> GetAllAsync();
    Task<IEnumerable<Resume>> GetByCandidateIdAsync(int candidateId);
    Task<Resume> AddAsync(Resume resume);
    Task UpdateAsync(Resume resume);
}
