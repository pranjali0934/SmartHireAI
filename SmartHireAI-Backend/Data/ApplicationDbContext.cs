using Microsoft.EntityFrameworkCore;
using SmartHireAI.Backend.Entities;

namespace SmartHireAI.Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<ResumeMatch> ResumeMatches => Set<ResumeMatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Resume>()
            .HasOne(r => r.Candidate)
            .WithMany(c => c.Resumes)
            .HasForeignKey(r => r.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResumeMatch>()
            .HasOne(rm => rm.Resume)
            .WithMany(r => r.ResumeMatches)
            .HasForeignKey(rm => rm.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResumeMatch>()
            .HasOne(rm => rm.Job)
            .WithMany()
            .HasForeignKey(rm => rm.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
