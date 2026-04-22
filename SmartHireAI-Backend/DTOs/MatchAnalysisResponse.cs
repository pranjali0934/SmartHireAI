namespace SmartHireAI.Backend.DTOs;

public class MatchAnalysisResponse
{
    public int ResumeId { get; set; }
    public int JobId { get; set; }
    public int MatchScore { get; set; }
    public string MatchingSkills { get; set; } = string.Empty;
    public string MissingSkills { get; set; } = string.Empty;
    public string? AIAnalysis { get; set; }
}
