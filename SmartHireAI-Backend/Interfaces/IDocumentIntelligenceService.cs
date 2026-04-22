namespace SmartHireAI.Backend.Interfaces;

public interface IDocumentIntelligenceService
{
    Task<string> ExtractTextFromDocumentAsync(Stream documentStream, string fileName);
}
