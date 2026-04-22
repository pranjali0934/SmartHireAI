namespace SmartHireAI.Backend.Entities;

public class ResumeMatch
{
    public int Id { get; set; }
    public int ResumeId { get; set; }
    public int JobId { get; set; }
    public int MatchScore { get; set; }
    public string MatchingSkills { get; set; } = string.Empty;
    public string MissingSkills { get; set; } = string.Empty;
    public string? AIAnalysis { get; set; }

    public Resume Resume { get; set; } = null!;
    public Job Job { get; set; } = null!;
}
