namespace SmartHireAI.Backend.AIIntegration;

public class AzureSettings
{
    public const string SectionName = "Azure";

    public string DocumentIntelligenceEndpoint { get; set; } = string.Empty;
    public string DocumentIntelligenceKey { get; set; } = string.Empty;
    public string OpenAIEndpoint { get; set; } = string.Empty;
    public string OpenAIKey { get; set; } = string.Empty;
    public string OpenAIDeploymentName { get; set; } = "gpt-4o";
}
