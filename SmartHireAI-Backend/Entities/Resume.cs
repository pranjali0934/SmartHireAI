namespace SmartHireAI.Backend.Entities;

public class Resume
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? ExtractedText { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Candidate Candidate { get; set; } = null!;
    public ICollection<ResumeMatch> ResumeMatches { get; set; } = new List<ResumeMatch>();
}
