namespace SmartHireAI.Backend.DTOs;

public class RankedCandidateDto
{
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ResumeId { get; set; }
    public int JobId { get; set; }
    public int MatchScore { get; set; }
    public string MatchingSkills { get; set; } = string.Empty;
    public string MissingSkills { get; set; } = string.Empty;
    public string? AIAnalysis { get; set; }
}
