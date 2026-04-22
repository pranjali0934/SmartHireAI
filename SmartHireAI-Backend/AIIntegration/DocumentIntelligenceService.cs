using System.Text;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Core;
using Microsoft.Extensions.Options;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.AIIntegration;

public class DocumentIntelligenceService : IDocumentIntelligenceService
{
    private readonly DocumentIntelligenceClient? _client;
    private readonly bool _isConfigured;

    public DocumentIntelligenceService(IOptions<AzureSettings> options)
    {
        var settings = options.Value;
        _isConfigured = !string.IsNullOrEmpty(settings.DocumentIntelligenceEndpoint) &&
                        !string.IsNullOrEmpty(settings.DocumentIntelligenceKey);

        if (_isConfigured)
        {
            _client = new DocumentIntelligenceClient(
                new Uri(settings.DocumentIntelligenceEndpoint),
                new AzureKeyCredential(settings.DocumentIntelligenceKey));
        }
    }

    public async Task<string> ExtractTextFromDocumentAsync(Stream documentStream, string fileName)
    {
        if (!_isConfigured || _client == null)
        {
            return await FallbackExtractionAsync(documentStream, fileName);
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await documentStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var content = new BinaryData(memoryStream.ToArray());
            var options = new AnalyzeDocumentOptions("prebuilt-read", content);

            var operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                options);

            var result = operation.Value;
            var text = new StringBuilder();

            if (result?.Content != null)
            {
                text.Append(result.Content);
            }
            else if (result?.Pages != null)
            {
                foreach (var page in result.Pages)
                {
                    if (page.Lines != null)
                    {
                        foreach (var line in page.Lines)
                        {
                            text.AppendLine(line.Content);
                        }
                    }
                }
            }

            return text.ToString().Trim();
        }
        catch (RequestFailedException)
        {
            return await FallbackExtractionAsync(documentStream, fileName);
        }
    }

    private static async Task<string> FallbackExtractionAsync(Stream documentStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (extension == ".txt")
        {
            using var reader = new StreamReader(documentStream);
            return await reader.ReadToEndAsync();
        }

        return "Document text extraction requires Azure Document Intelligence to be configured. " +
               "Please add DocumentIntelligenceEndpoint and DocumentIntelligenceKey to appsettings.json.";
    }
}
