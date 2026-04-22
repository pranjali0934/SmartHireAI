using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Data;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Repositories;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(int id) =>
        await _context.Jobs.FindAsync(id);

    public async Task<IEnumerable<Job>> GetAllAsync() =>
        await _context.Jobs.OrderByDescending(j => j.CreatedAt).ToListAsync();

    public async Task<Job> AddAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }
}
