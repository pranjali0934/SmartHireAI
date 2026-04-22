using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Data;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly ApplicationDbContext _context;

    public ResumeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Resume?> GetByIdAsync(int id) =>
        await _context.Resumes
            .Include(r => r.Candidate)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Resume>> GetAllAsync() =>
        await _context.Resumes
            .Include(r => r.Candidate)
            .OrderByDescending(r => r.UploadedAt)
            .ToListAsync();

    public async Task<IEnumerable<Resume>> GetByCandidateIdAsync(int candidateId) =>
        await _context.Resumes
            .Where(r => r.CandidateId == candidateId)
            .OrderByDescending(r => r.UploadedAt)
            .ToListAsync();

    public async Task<Resume> AddAsync(Resume resume)
    {
        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();
        return resume;
    }

    public async Task UpdateAsync(Resume resume)
    {
        _context.Resumes.Update(resume);
        await _context.SaveChangesAsync();
    }
}
