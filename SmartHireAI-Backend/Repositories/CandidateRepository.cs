using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Data;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly ApplicationDbContext _context;

    public CandidateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Candidate?> GetByIdAsync(int id) =>
        await _context.Candidates
            .Include(c => c.Resumes)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Candidate?> GetByEmailAsync(string email) =>
        await _context.Candidates
            .Include(c => c.Resumes)
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

    public async Task<IEnumerable<Candidate>> GetAllAsync() =>
        await _context.Candidates
            .Include(c => c.Resumes)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

    public async Task<Candidate> AddAsync(Candidate candidate)
    {
        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();
        return candidate;
    }
}
