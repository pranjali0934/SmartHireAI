using SmartHireAI.Backend.Entities;

namespace SmartHireAI.Backend.Interfaces;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(int id);
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job> AddAsync(Job job);
}
