using System.Text.Json;
using System.Text.RegularExpressions;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.AIIntegration;

public class OpenAIAnalysisService : IOpenAIService
{
    private readonly OpenAIClient? _client;
    private readonly string? _deploymentName;
    private readonly bool _isConfigured;

    public OpenAIAnalysisService(IOptions<AzureSettings> options)
    {
        var settings = options.Value;
        _isConfigured = !string.IsNullOrEmpty(settings.OpenAIEndpoint) &&
                        !string.IsNullOrEmpty(settings.OpenAIKey) &&
                        !string.IsNullOrEmpty(settings.OpenAIDeploymentName);

        if (_isConfigured)
        {
            _client = new OpenAIClient(
                new Uri(settings.OpenAIEndpoint),
                new AzureKeyCredential(settings.OpenAIKey));
            _deploymentName = settings.OpenAIDeploymentName;
        }
    }

    public async Task<ResumeMatchResult> AnalyzeResumeMatchAsync(
        string resumeText,
        string jobTitle,
        string jobDescription,
        string requiredSkills)
    {
        if (!_isConfigured || _client == null || string.IsNullOrEmpty(_deploymentName))
        {
            return GetFallbackResult(resumeText, requiredSkills);
        }

        var prompt = $@"Analyze the following resume and job description.
Extract candidate skills, compare with job requirements, calculate a match score (0-100), list matching skills and missing skills.

RESPOND WITH A JSON OBJECT in this exact format (no other text):
{{
  ""matchScore"": <number 0-100>,
  ""matchingSkills"": ""comma-separated list of matching skills"",
  ""missingSkills"": ""comma-separated list of missing skills"",
  ""analysis"": ""brief analysis paragraph""
}}

---
RESUME:
{resumeText}

---
JOB TITLE: {jobTitle}
JOB DESCRIPTION: {jobDescription}
REQUIRED SKILLS: {requiredSkills}
---";

        try
        {
            var options = new ChatCompletionsOptions
            {
                DeploymentName = _deploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are an expert HR analyst. Respond only with valid JSON."),
                    new ChatRequestUserMessage(prompt)
                },
                MaxTokens = 1024
            };

            var response = await _client.GetChatCompletionsAsync(options);
            var content = response.Value.Choices[0].Message.Content ?? "";

            return ParseAIResponse(content);
        }
        catch
        {
            return GetFallbackResult(resumeText, requiredSkills);
        }
    }

    private static ResumeMatchResult ParseAIResponse(string content)
    {
        try
        {
            var jsonMatch = Regex.Match(content, @"\{[\s\S]*\}");
            if (jsonMatch.Success)
            {
                var json = jsonMatch.Value;
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var matchScore = root.TryGetProperty("matchScore", out var scoreProp)
                    ? Math.Clamp(scoreProp.GetInt32(), 0, 100)
                    : 50;

                var matchingSkills = root.TryGetProperty("matchingSkills", out var matchProp)
                    ? matchProp.GetString() ?? ""
                    : "";

                var missingSkills = root.TryGetProperty("missingSkills", out var missProp)
                    ? missProp.GetString() ?? ""
                    : "";

                var analysis = root.TryGetProperty("analysis", out var analysisProp)
                    ? analysisProp.GetString() ?? ""
                    : "";

                return new ResumeMatchResult(matchScore, matchingSkills, missingSkills, analysis);
            }
        }
        catch { /* Fall through to fallback */ }

        return new ResumeMatchResult(50, "", "", "AI analysis could not be parsed.");
    }

    private static ResumeMatchResult GetFallbackResult(string resumeText, string requiredSkills)
    {
        var requiredList = requiredSkills.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var resumeLower = resumeText.ToLowerInvariant();
        var matching = new List<string>();
        var missing = new List<string>();

        foreach (var skill in requiredList)
        {
            if (string.IsNullOrWhiteSpace(skill)) continue;
            if (resumeLower.Contains(skill.Trim().ToLowerInvariant()))
                matching.Add(skill.Trim());
            else
                missing.Add(skill.Trim());
        }

        var score = requiredList.Length > 0
            ? (int)Math.Round((double)matching.Count / requiredList.Length * 100)
            : 50;

        return new ResumeMatchResult(
            Math.Clamp(score, 0, 100),
            string.Join(", ", matching),
            string.Join(", ", missing),
            "Azure OpenAI not configured. Match score calculated using simple keyword matching.");
    }
}
