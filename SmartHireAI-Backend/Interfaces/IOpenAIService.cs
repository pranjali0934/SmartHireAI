namespace SmartHireAI.Backend.Interfaces;

public interface IOpenAIService
{
    Task<ResumeMatchResult> AnalyzeResumeMatchAsync(string resumeText, string jobTitle, string jobDescription, string requiredSkills);
}

public record ResumeMatchResult(
    int MatchScore,
    string MatchingSkills,
    string MissingSkills,
    string AIAnalysis);
