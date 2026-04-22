namespace SmartHireAI.Backend.DTOs;

public class ResumeUploadResponse
{
    public int ResumeId { get; set; }
    public int CandidateId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? ExtractedText { get; set; }
    public DateTime UploadedAt { get; set; }
}
