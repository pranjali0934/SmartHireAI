namespace SmartHireAI.Backend.DTOs;

public class DashboardStatsDto
{
    public int TotalResumes { get; set; }
    public int TotalJobs { get; set; }
    public int TotalCandidates { get; set; }
    public List<RankedCandidateDto> TopCandidates { get; set; } = new();
}
